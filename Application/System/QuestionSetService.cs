using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System;

namespace Application.System
{
    public class QuestionSetService : IQuestionSetService
    {
        private readonly AppDbContext _context;

        public QuestionSetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> Create(QuestionSetCreateRequest request)
        {
            var check = await _context.QuestionSets.AnyAsync(x => x.Name == request.Name);
            if (check) return new ApiResult("Name is existed");

            var questionSet = new QuestionSet
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Image = request.Image,
                Questions = request.Questions.Select(x => new Question
                {
                    Content = x.Content,
                    Image = x.Image,
                    AnswerA = x.AnswerA,
                    AnswerB = x.AnswerB,
                    AnswerC = x.AnswerC,
                    AnswerD = x.AnswerD,
                    CorrectAnswer = x.CorrectAnswer,
                    Score = x.Score,
                    Mark = false
                }).ToList()
            };

            try
            {
                _context.QuestionSets.Add(questionSet);
                await _context.SaveChangesAsync();
                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message);
            }

        }

        public async Task<ApiResult> Delete(Guid id)
        {
            var questionSet = await _context.QuestionSets.FindAsync(id);
            if (questionSet == null) return new ApiResult("Not found");

            try
            {
                _context.QuestionSets.Remove(questionSet);
                await _context.SaveChangesAsync();
                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message);
            }
        }

        public async Task<ApiResult> GetAll()
        {
            var questionSets = await _context.QuestionSets.Select(x => new QuestionSetViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                Image = x.Image,
                NumberOfQuestions = x.Questions.Count
            }).ToListAsync();

            return new ApiResult(questionSets);
        }

        public async Task<ApiResult> GetById(Guid id)
        {
            var questionSet = await _context.QuestionSets.FindAsync(id);
            if (questionSet == null) return new ApiResult("Not found");
            return new ApiResult(questionSet);
        }

        public async Task<ApiResult> Update(Guid id, QuestionSetCreateRequest request)
        {
            var questionSet = await _context.QuestionSets.FindAsync(id);
            if (questionSet == null) return new ApiResult("Not found");

            questionSet.Name = request.Name;
            questionSet.Description = request.Description;
            questionSet.Image = request.Image;
            questionSet.Questions = request.Questions.Select(x => new Question
            {
                Content = x.Content,
                Image = x.Image,
                AnswerA = x.AnswerA,
                AnswerB = x.AnswerB,
                AnswerC = x.AnswerC,
                AnswerD = x.AnswerD,
                CorrectAnswer = x.CorrectAnswer,
                Score = x.Score,
                Mark = false
            }).ToList();

            try
            {
                _context.Update(questionSet);
                await _context.SaveChangesAsync();
                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message);
            }
        }
    }
}
