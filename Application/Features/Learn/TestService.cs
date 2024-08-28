using Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Application.Common;
using Utilities.Exceptions;
using ViewModels.Features.Learn.Test;
using ViewModels.Features.Learn.Test.Question;
using ViewModels.Common;

namespace Application.Features.Learn
{
    public class TestService : ITestService
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public TestService(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task Create(CreateTestRequest request)
        {
            //check if test name existed
            var check = await _context.Tests.AnyAsync(x => x.Name == request.Name);
            if (check)
                throw new BadRequestException("Tên bài test bị trùng");

            //create new question set
            var test = new Test
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                AuthorName = request.AuthorName,
                AuthorId = Guid.Parse(request.AuthorId),
                CreatedDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                UpdatedDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                Source = request.Source,
                ImageUrl = await _fileService.SaveFile(request.Image),
                Duration = request.Duration,
                NumberOfAttempts = 0,
                NumberOfQuestions = request.Questions.Count,
            };

            //create questions
            var questions = new List<Question>();
            if (request.Questions == null)
                throw new BadRequestException("Danh sách câu hỏi trống");
            foreach (var question in request.Questions)
            {
                var newQuestion = new Question
                {
                    Id = Guid.NewGuid(),
                    Content = question.Content,
                    ImageUrl = await _fileService.SaveFile(question.Image),
                    AnswerA = question.AnswerA,
                    AnswerB = question.AnswerB,
                    AnswerC = question.AnswerC,
                    AnswerD = question.AnswerD,
                    CorrectAnswer = question.CorrectAnswer,
                    TestId = test.Id,
                };
                questions.Add(newQuestion);
            }
            test.Questions = questions;

            _context.Tests.Add(test);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var test = await _context.Tests.FindAsync(Guid.Parse(id));
            if (test == null)
                throw new NotFoundException("Test không tồn tại");

            await _fileService.DeleteFile(test.ImageUrl);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
        }

        public async Task<PagingResponse<TestItem>> GetAll(PagingRequest request)
        {
            var tests = await _context.Tests
                .Where(x => request.Key == null || x.Name.Contains(request.Key))
                .Select(x => new TestItem
                {
                    Id = x.Id.ToString().ToLower(),
                    Name = x.Name,
                    ImageUrl = _fileService.GetFileUrl(x.ImageUrl),
                    NumberOfAttempts = x.NumberOfAttempts,
                    NumberOfQuestions = x.NumberOfQuestions
                }).ToListAsync();

            return new PagingResponse<TestItem>
            {
                Total = tests.Count,
                Data = tests
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList()
            };
        }

        public async Task<PagingResponse<TestResult>> GetAllResults(PagingRequest request)
        {
            var results = await _context.TestResults
                .Where(
                x => request.Key == null ||
                x.TestName.Contains(request.Key) ||
                x.UserName.Contains(request.Key))
                .ToListAsync();

            return new PagingResponse<TestResult>
            {
                Total = results.Count,
                Data = results
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList()
            };
        }

        public async Task<TestResponse> GetTestContentById(string id)
        {
            var test = await _context.Tests.FindAsync(Guid.Parse(id));
            if (test == null)
            {
                throw new BadRequestException("Test không tồn tại");
            }

            var questions = await _context.Questions
                .Where(x => x.TestId == test.Id)
                .Select(x => new QuestionResponse
                {
                    Id = x.Id.ToString(),
                    Content = x.Content,
                    ImageUrl = _fileService.GetFileUrl(x.ImageUrl),
                    AnswerA = x.AnswerA,
                    AnswerB = x.AnswerB,
                    AnswerC = x.AnswerC,
                    AnswerD = x.AnswerD,
   
                }).ToListAsync();

            return new TestResponse
            {
                Name = test.Name,
                Duration = test.Duration,
                Questions = questions
            };
        }

        public async Task<TestDetailResponse> GetDetailById(string id)
        {
            var test = await _context.Tests.FindAsync(Guid.Parse(id));
            if (test == null)
            {
                throw new BadRequestException("Test không tồn tại");
            }

            return new TestDetailResponse
            {
                Id = test.Id.ToString(),
                Name = test.Name,
                ImageUrl = test.ImageUrl,
                UpdatedDate = test.UpdatedDate,
                CreatedDate = test.CreatedDate,
                Description = test.Description,
                Source = test.Source,
                AuthorName = test.AuthorName,
                AuthorId = test.AuthorId.ToString(),
                NumberOfAttempts = test.NumberOfAttempts,
                NumberOfQuestions = test.NumberOfQuestions
            };
        }

        public async Task<TestResultResponse> MarkTest(MarkTestRequest request)
        {
            var questions = await _context.Questions
                .Where(x => x.TestId.ToString() == request.TestId)
                .ToListAsync();

            //mark correct ans
            Dictionary<string, int> map = new();
            foreach (var e in questions)
            {
                map.Add(e.Id.ToString(), e.CorrectAnswer);
            }

            //count number of correct and unselected ans
            int correct = 0;
            int unselected = 0;
            foreach (var e in request.Answers)
            {
                if (e.Selected == 0)
                {
                    unselected++;
                    continue;
                }

                if (e.Selected == map[e.Id.ToLower()])
                {
                    correct++;
                }

            }

            //calculate used time
            var start = DateTime.Parse(request.StartTime);
            var end = DateTime.Parse(request.EndTime);
            var duration = end - start;

            //calculate score and save result
            double score = (double)correct / questions.Count * 10;
            _context.TestResults.Add(new TestResult
            {
                Id = Guid.NewGuid(),
                Score = (decimal)score,
                Correct = correct,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                UsedTime = (int)duration.TotalSeconds,
                TestId = Guid.Parse(request.TestId),
                TestName = request.TestName,
                UserId = Guid.Parse(request.UserId),
                UserName = request.UserName
            });
            await _context.SaveChangesAsync();

            //return result to user
            return new TestResultResponse
            {
                Total = questions.Count,
                Score = score,
                Correct = correct,
                Unselected = unselected,
                UsedTime = (int)duration.TotalSeconds,
                Detail = request.Answers.Select(x => map[x.Id.ToLower()]).ToList()
            };
        }

        public async Task RemoveAllResults()
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM TestResults");
        }

        public async Task Update(string id, TestUpdateRequest request)
        {
            //check if test existed
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
                throw new BadRequestException("Test không tồn tại");


            //update
            test.Name = request.Name;
            await _fileService.DeleteFile(test.ImageUrl);
            test.ImageUrl = await _fileService.SaveFile(request.Image);
            test.Description = request.Description;
            test.Source = request.Source;
            test.Duration = request.Duration;
            test.UpdatedDate = DateOnly.FromDateTime(DateTime.Now).ToString();

            //remove all old question associate with this test
            var oldQuestions = await _context.Questions
                .Where(q => q.TestId == test.Id)
                .ToListAsync();
            foreach (var e in oldQuestions)
            {
                await _fileService.DeleteFile(e.ImageUrl);
            }
            _context.Questions.RemoveRange(oldQuestions);

            //add new questions
            var newQuestions = new List<Question>();
            foreach (var question in request.Questions)
            {
                var newQuestion = new Question
                {
                    Content = question.Content,
                    ImageUrl = await _fileService.SaveFile(question.Image),
                    AnswerA = question.AnswerA,
                    AnswerB = question.AnswerB,
                    AnswerC = question.AnswerC,
                    AnswerD = question.AnswerD,
                    CorrectAnswer = question.CorrectAnswer,
                    TestId = test.Id,
                };
                newQuestions.Add(newQuestion);
            }
            test.Questions = newQuestions;

            _context.Tests.Update(test);
            await _context.SaveChangesAsync();
        }
    }
}