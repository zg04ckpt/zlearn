using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Question;

namespace Application.Practice
{
    public class QuestionService : IQuestionServices
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> GetByQuestionSetId(string id)
        {
            try
            {
                var questions = await _context.Questions
                .Where(x => x.QuestionSetId == Guid.Parse(id))
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
                return new ApiResult(questions);
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
