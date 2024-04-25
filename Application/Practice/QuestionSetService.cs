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

namespace Application.Practice
{
    public class QuestionSetService : IQuestionSetService
    {
        private readonly AppDbContext _context;

        public QuestionSetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> Create(QSCreateRequest request)
        {
            try
            {
                var check = await _context.QuestionSets.AnyAsync(x => x.Name == request.Name);
                if (check) return new ApiResult("Question set name is existed", HttpStatusCode.BadRequest);

                var questionSet = new QuestionSet
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Creator = request.Creator,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    ImageUrl = request.ImageUrl,
                    Questions = request.Questions.Select(x => new Question
                    {
                        Content = x.Content,
                        ImageUrl = x.ImageUrl,
                        AnswerA = x.AnswerA,
                        AnswerB = x.AnswerB,
                        AnswerC = x.AnswerC,
                        AnswerD = x.AnswerD,
                        CorrectAnswer = x.CorrectAnswer,
                        Score = 0,
                        Mark = x.Mark
                    }).ToList()
                };

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
                    .Select(qs => new QSResponse
                    {
                        Id = qs.Id,
                        Name = qs.Name,
                        Description = qs.Description,
                        Creator = qs.Creator,
                        CreatedDate = qs.CreatedDate,
                        UpdatedDate = qs.UpdatedDate,
                        ImageUrl = qs.ImageUrl,
                        QuestionCount = qs.Questions.Count
                    }).ToListAsync();

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
                    ImageUrl = questionSet.ImageUrl,
                    QuestionCount = await _context.Questions.CountAsync(q => q.QuestionSetId == questionSet.Id)
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
                var questionSet = await _context.QuestionSets.FindAsync(Guid.Parse(id));
                if (questionSet == null) return new ApiResult("Not found", HttpStatusCode.NotFound);

                questionSet.Questions = await _context.Questions
                    .Where(q => q.QuestionSetId == questionSet.Id)
                    .ToListAsync();

                questionSet.Name = request.Name;
                questionSet.Description = request.Description;
                questionSet.UpdatedDate = DateTime.Now;
                questionSet.ImageUrl = request.ImageUrl;

                var oldQuestions = await _context.Questions
                    .Where(q => q.QuestionSetId == questionSet.Id)
                    .ToListAsync();
                _context.Questions.RemoveRange(oldQuestions);

                var newQuestions = request.Questions.Select(x => new Question
                {
                    Content = x.Content,
                    ImageUrl = x.ImageUrl,
                    AnswerA = x.AnswerA,
                    AnswerB = x.AnswerB,
                    AnswerC = x.AnswerC,
                    AnswerD = x.AnswerD,
                    CorrectAnswer = x.CorrectAnswer,
                    QuestionSetId = questionSet.Id,
                    Score = 0,
                    Mark = x.Mark
                }).ToList();
                _context.Questions.AddRange(newQuestions);

                await _context.SaveChangesAsync();
                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
