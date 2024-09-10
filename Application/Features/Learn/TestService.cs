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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        #region main
        public async Task Create(CreateTestRequest request, ClaimsPrincipal user)
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
                AuthorName = user.FindFirst(ClaimTypes.GivenName).Value,
                AuthorId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value),
                CreatedDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                UpdatedDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                Source = request.Source,
                ImageUrl = await _fileService.SaveFile(request.Image),
                Duration = request.Duration,
                NumberOfAttempts = 0,
                NumberOfQuestions = request.Questions.Count,
                IsPrivate = request.IsPrivate,
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

        public async Task Delete(string testId, string userId)
        {
            var test = await _context.Tests.FindAsync(Guid.Parse(testId))
            ?? throw new BadRequestException("Test không tồn tại");

            if(test.AuthorId.ToString() != userId)
            {
                throw new ForbiddenException();
            }

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
                    NumberOfQuestions = x.NumberOfQuestions,
                    IsPrivate = x.IsPrivate,
                }).ToListAsync();

            return new PagingResponse<TestItem>
            {
                Total = tests.Count,
                Data = tests
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList()
            };
        }

        public async Task<TestResponse> GetTestContentById(ClaimsPrincipal user, string testId)
        {
            var test = await _context.Tests.FindAsync(Guid.Parse(testId)) 
                ?? throw new BadRequestException("Test không tồn tại");

            if(test.IsPrivate)
            {
                //check if user hasn't owner this test
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (test.AuthorId.ToString() != userId)
                {
                    throw new ForbiddenException();
                }   
                // next update: can allow user to access with link
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
            var test = await _context.Tests.FindAsync(Guid.Parse(id))
                ?? throw new BadRequestException("Test không tồn tại");

            return new TestDetailResponse
            {
                Id = test.Id.ToString(),
                Name = test.Name,
                ImageUrl = _fileService.GetFileUrl(test.ImageUrl),
                UpdatedDate = test.UpdatedDate,
                CreatedDate = test.CreatedDate,
                Description = test.Description,
                Source = test.Source,
                AuthorName = test.AuthorName,
                AuthorId = test.AuthorId.ToString(),
                NumberOfAttempts = test.NumberOfAttempts,
                NumberOfQuestions = test.NumberOfQuestions,
                IsPrivate = test.IsPrivate
            };
        }

        public async Task<TestResultResponse> MarkTest(MarkTestRequest request, string ip, ClaimsPrincipal user)
        {
            var test = await _context.Tests.FindAsync(Guid.Parse(request.TestId))
                ?? throw new BadRequestException("Test không tồn tại");
            test.NumberOfAttempts++;

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
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? ip,
                UserName = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "undefined"
            }) ;
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

        public async Task<PagingResponse<TestResult>> GetAllResults(string userId, PagingRequest request)
        {
            var results = await _context.TestResults
                .Where(
                x => x.UserId == userId &&
                (request.Key == null ||
                x.TestName.Contains(request.Key) ||
                x.UserName.Contains(request.Key)))
                .ToListAsync();

            return new PagingResponse<TestResult>
            {
                Total = results.Count,
                Data = results
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList()
            };
        }

        public Task RemoveAllResults()
        {
            //await _context.Database.ExecuteSqlRawAsync("DELETE FROM TestResults");
            throw new BadRequestException("This feature is unavailable");
        }

        public async Task Update(string userId, string testId, TestUpdateRequest request)
        {
            //check if test existed
            var test = await _context.Tests.FindAsync(Guid.Parse(testId)) 
                ?? throw new BadRequestException("Test không tồn tại");
            if (test.AuthorId.ToString() != userId)
            {
                throw new ForbiddenException();
            }

            //update
            test.Name = request.Name;
            await _fileService.DeleteFile(test.ImageUrl);
            test.ImageUrl = await _fileService.SaveFile(request.Image);
            test.Description = request.Description;
            test.Source = request.Source;
            test.Duration = request.Duration;
            test.UpdatedDate = DateOnly.FromDateTime(DateTime.Now).ToString();
            test.IsPrivate = request.IsPrivate;
            _context.Tests.Update(test);

            //update question
            Dictionary<string, QuestionUpdateRequest> map = new();
            foreach(var e in request.Questions)
            {
                if(e.Id == null)
                {
                    var newQ = new Question
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = await _fileService.SaveFile(e.Image),
                        Content = e.Content,
                        AnswerA = e.AnswerA,
                        AnswerB = e.AnswerB,
                        AnswerC = e.AnswerC,
                        AnswerD = e.AnswerD,
                        CorrectAnswer = e.CorrectAnswer,
                        TestId = test.Id,
                    };
                    await _context.Questions.AddAsync(newQ);
                } 
                else
                {
                    map.Add(e.Id, e);
                }
            }
            var oldQuestions = await _context.Questions
                .Where(q => q.TestId == test.Id)
                .ToListAsync();
            foreach (var e in oldQuestions)
            {
                if(map.TryGetValue(e.Id.ToString(), out var updated))
                {
                    await _fileService.DeleteFile(e.ImageUrl);
                    e.ImageUrl = await _fileService.SaveFile(updated.Image);
                    e.Content = updated.Content;
                    e.AnswerA = updated.AnswerA;
                    e.AnswerB = updated.AnswerB;
                    e.AnswerC = updated.AnswerC;
                    e.AnswerD = updated.AnswerD;
                    e.CorrectAnswer = updated.CorrectAnswer;
                    _context.Questions.Update(e);
                }
                else
                {
                    _context.Questions.Remove(e);
                }    
            }

            //save all changes
            await _context.SaveChangesAsync();
        }

        public async Task<List<TestDetailResponse>> GetTestsByUserId(string userId)
        {
            return await _context.Tests
                .Where(x => x.AuthorId.ToString() == userId)
                .Select(test => new TestDetailResponse
                {
                    Id = test.Id.ToString(),
                    Name = test.Name,
                    ImageUrl = _fileService.GetFileUrl(test.ImageUrl),
                    UpdatedDate = test.UpdatedDate,
                    CreatedDate = test.CreatedDate,
                    Description = test.Description,
                    Source = test.Source,
                    AuthorName = test.AuthorName,
                    AuthorId = test.AuthorId.ToString(),
                    NumberOfAttempts = test.NumberOfAttempts,
                    NumberOfQuestions = test.NumberOfQuestions
                }).ToListAsync();

        }

        public async Task SaveTest(string userId, string testId)
        {
            var test = await _context.Tests.FindAsync(Guid.Parse(testId))
                ?? throw new BadRequestException("Test không tồn tại");

            await _context.SavedTest.AddAsync(new SavedTest
            {
                UserId = Guid.Parse(userId),
                TestId = Guid.Parse(testId),
                MarkedAt = DateOnly.FromDateTime(DateTime.Now).ToString()
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<TestItem>> GetSavedTestsByUserId(string userId)
        {
            return await (from test in _context.Tests
                          join uit in _context.SavedTest on test.Id equals uit.TestId
                          where uit.UserId.ToString() == userId
                          select new TestItem
                          {
                              Id = test.Id.ToString().ToLower(),
                              Name = test.Name,
                              ImageUrl = _fileService.GetFileUrl(test.ImageUrl),
                              NumberOfAttempts = test.NumberOfAttempts,
                              NumberOfQuestions = test.NumberOfQuestions,
                              IsPrivate = test.IsPrivate,
                          }).ToListAsync();
                
        } 
        
        public async Task<TestUpdateContent> GetTestUpdateContent(string userId, string testId)
        {
            var test = await _context.Tests.FindAsync(Guid.Parse(testId)) 
                ?? throw new BadRequestException("Test không tồn tại");

            if(test.AuthorId.ToString() != userId)
            {
                throw new ForbiddenException();
            }

            var question = await _context.Questions
                .Where(x => x.TestId.ToString() == testId)
                .Select(x => new QuestionUpdateContent
                {
                    Id = x.Id.ToString().ToLower(),
                    Content = x.Content,
                    ImageUrl = _fileService.GetFileUrl(x.ImageUrl),
                    AnswerA = x.AnswerA,
                    AnswerB = x.AnswerB,
                    AnswerC = x.AnswerC,
                    AnswerD = x.AnswerD,
                    CorrectAnswer = x.CorrectAnswer
                }).ToListAsync();

            return new TestUpdateContent
            {
                Name = test.Name,
                ImageUrl = _fileService.GetFileUrl(test.ImageUrl),
                Description = test.Description,
                Source = test.Source,
                Duration = test.Duration,
                IsPrivate = test.IsPrivate,
                Questions = question,
            };
        }

        public async Task DeleteFromSaved(string userId, string testId)
        {
            var uit = await _context.SavedTest
                .Where(x => x.UserId.ToString() == userId && x.TestId.ToString() == testId)
                .FirstOrDefaultAsync() ?? throw new BadRequestException("Không tìm thấy item");
            _context.SavedTest.Remove(uit);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsSaved(string userId, string testId)
        {
            return await _context.SavedTest
                .AnyAsync(x => x.UserId.ToString() == userId && x.TestId.ToString() == testId);
        }
        #endregion
    }
}