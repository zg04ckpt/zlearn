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

        public async Task<ApiResult> Create(QSCreateRequest request)
        {
            try
            {
                //check if question set name is existed
                var check = await _context.QuestionSets.AnyAsync(x => x.Name == request.Name);
                if (check) return new ApiResult("Question set name is existed", HttpStatusCode.BadRequest);

                //create new question set
                var questionSet = new QuestionSet
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
                if(request.Questions != null) //test
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
                            QuestionSetId = questionSet.Id,
                            Score = 0,
                            Mark = question.Mark
                        };
                        questions.Add(newQuestion);
                    }
                questionSet.Questions = questions;

                _context.QuestionSets.Add(questionSet);
                await _context.SaveChangesAsync();

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult> Delete(string id)
        {
            try
            {
                var questionSet = await _context.QuestionSets.FindAsync(Guid.Parse(id));
                if (questionSet == null) return new ApiResult("Not found", HttpStatusCode.NotFound);

                await _fileService.DeleteFile(questionSet.ImageUrl);

                _context.QuestionSets.Remove(questionSet);
                await _context.SaveChangesAsync();
                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult> GetAll()
        {
            try
            {
                var questionSets = await _context.QuestionSets
                    .Include(qs => qs.Questions)
                    .Include(qs => qs.TestResults)
                    .Select(qs => new QSResponse
                    {
                        Id = qs.Id,
                        Name = qs.Name,
                        Description = qs.Description,
                        Creator = qs.Creator,
                        CreatedDate = qs.CreatedDate,
                        UpdatedDate = qs.UpdatedDate,
                        ImageUrl = _fileService.GetFileUrl(qs.ImageUrl),
                        AttemptCount = qs.TestResults.Count,
                        QuestionCount = qs.Questions.Count,
                        TestTime = new TestTime
                        {
                            Minutes = qs.TestTime.Minutes + qs.TestTime.Hours*60,
                            Seconds = qs.TestTime.Seconds
                        }
                    }).ToListAsync();
                questionSets.Sort((a, b) => a.Name.CompareTo(b.Name));

                return new ApiResult(questionSets);
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult> GetById(string id)
        {
            try
            {
                var questionSet = await _context.QuestionSets.FindAsync(Guid.Parse(id));
                if (questionSet == null)
                    return new ApiResult("Not found", HttpStatusCode.NotFound);

                return new ApiResult(new QSResponse
                {
                    Id = questionSet.Id,
                    Name = questionSet.Name,
                    Description = questionSet.Description,
                    Creator = questionSet.Creator,
                    CreatedDate = questionSet.CreatedDate,
                    UpdatedDate = questionSet.UpdatedDate,
                    ImageUrl = _fileService.GetFileUrl(questionSet.ImageUrl),
                    AttemptCount = await _context.TestResults.CountAsync(tr => tr.QuestionSetId == questionSet.Id),
                    QuestionCount = await _context.Questions.CountAsync(q => q.QuestionSetId == questionSet.Id),
                    TestTime = new TestTime
                    {
                        Minutes = questionSet.TestTime.Minutes + questionSet.TestTime.Hours * 60,
                        Seconds = questionSet.TestTime.Seconds
                    }
                });
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult> Update(string id, QSUpdateRequest request)
        {
            try
            {
                //check if question set is existed
                var questionSet = await _context.QuestionSets.FindAsync(Guid.Parse(id));
                if (questionSet == null) return new ApiResult("Not found", HttpStatusCode.NotFound);

                //get questions
                questionSet.Questions = await _context.Questions
                    .Where(q => q.QuestionSetId == questionSet.Id)
                    .ToListAsync();

                #region Update question set
                questionSet.Name = request.Name;
                questionSet.Description = request.Description;
                questionSet.UpdatedDate = DateTime.Now;
                questionSet.TestTime = new TimeSpan(
                        0,
                        request.TestTime.Minutes,
                        request.TestTime.Seconds
                    );

                //update image
                if (request.Image != null)
                {
                    await _fileService.DeleteFile(questionSet.ImageUrl);
                    questionSet.ImageUrl = await _fileService.SaveFile(request.Image);
                }    

                //remove old questions
                var oldQuestions = await _context.Questions
                    .Where(q => q.QuestionSetId == questionSet.Id)
                    .ToListAsync();
                _context.Questions.RemoveRange(oldQuestions);

                //add new questions
                var newQuestions = new List<Question>();
                foreach(var question in request.Questions)
                {
                    var newQuestion = new Question
                    {
                        Order = question.Order,
                        Content = question.Content,
                        ImageUrl = (question.Image != null)? await _fileService.SaveFile(question.Image) : null,
                        AnswerA = question.AnswerA,
                        AnswerB = question.AnswerB,
                        AnswerC = question.AnswerC,
                        AnswerD = question.AnswerD,
                        CorrectAnswer = question.CorrectAnswer,
                        QuestionSetId = questionSet.Id,
                        Score = 0,
                        Mark = question.Mark
                    };
                    newQuestions.Add(newQuestion);
                }
                questionSet.Questions = newQuestions;

                _context.QuestionSets.Update(questionSet);
                await _context.SaveChangesAsync();
                #endregion

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
