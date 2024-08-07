using Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using Data.Entities;
using System.Net;
using ViewModels.QuestionSet;
using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Utilities.Exceptions;
using ViewModels.Question;
using Microsoft.AspNetCore.Http;
using ViewModels.Test;

namespace Application.Practice
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

        public async Task<List<TestResponse>> GetAll()
        {
            var tests = await _context.Tests
                .Include(t => t.Questions)
                .Include(t => t.TestResults)
                .Select(t => new TestResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Creator = t.Creator,
                    CreatedDate = t.CreatedDate,
                    UpdatedDate = t.UpdatedDate,
                    ImageUrl = _fileService.GetFileUrl(t.ImageUrl),
                    AttemptCount = t.TestResults.Count,
                    QuestionCount = t.Questions.Count,
                    TestTime = new TestTime
                    {
                        Minutes = t.TestTime.Minutes + t.TestTime.Hours * 60,
                        Seconds = t.TestTime.Seconds
                    }
                }).ToListAsync();
            tests.Sort((a, b) => a.Name.CompareTo(b.Name));

            return tests;
        }
        public async Task<TestResponse> GetById(string id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
                throw new NotFoundException("Test không tồn tại");

            return new TestResponse
            {
                Id = test.Id,
                Name = test.Name,
                Description = test.Description,
                Creator = test.Creator,
                CreatedDate = test.CreatedDate,
                UpdatedDate = test.UpdatedDate,
                ImageUrl = _fileService.GetFileUrl(test.ImageUrl),
                AttemptCount = await _context.TestResults.CountAsync(tr => tr.TestId ==test.Id),
                QuestionCount = await _context.Questions.CountAsync(q => q.TestId ==test.Id),
                TestTime = new TestTime
                {
                    Minutes = test.TestTime.Minutes +test.TestTime.Hours * 60,
                    Seconds = test.TestTime.Seconds
                }
            };
        }
        public async Task<List<QuestionViewModel>> GetAllQuestionByTestId(string id)
        {
            return await _context.Questions
                .Where(x => x.TestId == Guid.Parse(id))
                .Select(x => new QuestionViewModel
                {
                    Order = x.Order,
                    Content = x.Content,
                    ImageUrl = x.ImageUrl,
                    AnswerA = x.AnswerA,
                    AnswerB = x.AnswerB,
                    AnswerC = x.AnswerC,
                    AnswerD = x.AnswerD,
                    CorrectAnswer = x.CorrectAnswer,
                    Mark = x.Mark
                }).ToListAsync();
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
                Creator = request.Creator,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                ImageUrl = await _fileService.SaveFile(request.Image),
                TestTime = new TimeSpan(
                    0,
                    request.TestTime.Minutes,
                    request.TestTime.Seconds
                )
            };

            //create questions
            var questions = new List<Question>();
            if (request.Questions != null) //test
                foreach (var question in request.Questions)
                {
                    var newQuestion = new Question
                    {
                        Order = question.Order,
                        Content = question.Content,
                        ImageUrl = (question.Image != null) ? await _fileService.SaveFile(question.Image) : null,
                        AnswerA = question.AnswerA,
                        AnswerB = question.AnswerB,
                        AnswerC = question.AnswerC,
                        AnswerD = question.AnswerD,
                        CorrectAnswer = question.CorrectAnswer,
                        TestId = test.Id,
                        Score = 0,
                        Mark = question.Mark
                    };
                    questions.Add(newQuestion);
                }
            test.Questions = questions;

            _context.Tests.Add(test);
            await _context.SaveChangesAsync();
        }
        public async Task Update(string id, TestUpdateRequest request)
        {
            //check if test existed
            var test = await _context.Tests.FindAsync(Guid.Parse(id));
            if (test == null) 
                throw new NotFoundException("Test không tồn tại");

            //get questions
            test.Questions = await _context.Questions
                .Where(q => q.TestId == test.Id)
                .ToListAsync();

            //update
            test.Name = request.Name;
            test.Description = request.Description;
            test.UpdatedDate = DateTime.Now;
            test.TestTime = new TimeSpan(
                0,
                request.TestTime.Minutes,
                request.TestTime.Seconds
            );

            //update image
            if (request.Image != null)
            {
                await _fileService.DeleteFile(test.ImageUrl);
                test.ImageUrl = await _fileService.SaveFile(request.Image);
            }

            //remove old questions
            var oldQuestions = await _context.Questions
                .Where(q => q.TestId == test.Id)
                .ToListAsync();
            _context.Questions.RemoveRange(oldQuestions);

            //add new questions
            var newQuestions = new List<Question>();
            foreach (var question in request.Questions)
            {
                var newQuestion = new Question
                {
                    Order = question.Order,
                    Content = question.Content,
                    ImageUrl = (question.Image != null) ? await _fileService.SaveFile(question.Image) : null,
                    AnswerA = question.AnswerA,
                    AnswerB = question.AnswerB,
                    AnswerC = question.AnswerC,
                    AnswerD = question.AnswerD,
                    CorrectAnswer = question.CorrectAnswer,
                    TestId = test.Id,
                    Score = 0,
                    Mark = question.Mark
                };
                newQuestions.Add(newQuestion);
            }
            test.Questions = newQuestions;

            _context.Tests.Update(test);
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
        public async Task<List<TestResultResponse>> GetAllResults()
        {
            return await _context.TestResults.Select(x => new TestResultResponse
            {
                Id = x.Id,
                Score = x.Score,
                CorrectsCount = x.CorrectsCount,
                UsedTime = new TestTime
                {
                    Minutes = x.UsedTime.Minutes,
                    Seconds = x.UsedTime.Seconds
                },
                StartTime = x.StartTime,
                UserInfo = x.UserInfo,
                QuestionSetId = x.TestId
            }).ToListAsync();
        }
        public async Task SaveResult(SaveTestResultRequest request, ConnectionInfo connectionInfo)
        {
            //chỉnh múi giờ
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            request.StartTime = TimeZoneInfo.ConvertTimeFromUtc(request.StartTime, cstZone);

            var testResult = new TestResult
            {
                Id = Guid.NewGuid(),
                Score = request.Score,
                CorrectsCount = request.CorrectsCount,
                UsedTime = new TimeSpan(0, request.UsedTime.Minutes, request.UsedTime.Seconds),
                StartTime = request.StartTime,
                UserInfo = request.UserInfo,
                Detail = $"IP: {connectionInfo.RemoteIpAddress}",
                TestId = request.QuestionSetId
            };
            await _context.TestResults.AddAsync(testResult);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveAllResults()
        {
            _context.TestResults.RemoveRange(_context.TestResults);
            await _context.SaveChangesAsync();
        }
    }
}
