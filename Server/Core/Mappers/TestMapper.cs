using Core.DTOs;
using Core.Services.Common;
using Data.Entities.TestEntities;

namespace Core.Mappers
{
    public class TestMapper
    {
        public static Test MapFromCreate(CreateTestDTO dto)
        {
            return new Test
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                UpdatedDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                Source = dto.Source,
                ImageUrl = dto.ImageUrl,
                Duration = dto.Duration,
                NumberOfAttempts = 0,
                NumberOfQuestions = dto.Questions.Count,
                IsPrivate = dto.IsPrivate,
                CategorySlug = dto.CategorySlug
            };
        }

        public static Test MapFromUpdate(Test test, UpdateTestDTO dto)
        {
            test.Name = dto.Name;
            test.Description = dto.Description;
            test.Source = dto.Source;
            test.Duration = dto.Duration;
            test.UpdatedDate = DateOnly.FromDateTime(DateTime.Now).ToString();
            test.IsPrivate = dto.IsPrivate;
            test.CategorySlug = dto.CategorySlug;
            return test;
        }

        public static UpdateTestDTO MapToUpdate(Test test, List<Question> questions)
        {
            return new UpdateTestDTO
            {
                Name = test.Name,
                ImageUrl = test.ImageUrl,
                Image = null,
                Description = test.Description,
                Source = test.Source,
                Duration = test.Duration,
                IsPrivate = test.IsPrivate,
                Questions = questions.Select(x => new UpdateQuestionDTO
                {
                    Id = x.Id.ToString().ToLower(),
                    Content = x.Content,
                    ImageUrl = x.ImageUrl,
                    Image = null,
                    AnswerA = x.AnswerA,
                    AnswerB = x.AnswerB,
                    AnswerC = x.AnswerC,
                    AnswerD = x.AnswerD,
                    CorrectAnswer = x.CorrectAnswer
                }).ToList(),
                CategorySlug = test.CategorySlug
            };
        }

        public static TestItemDTO MapToItem(Test test)
        {
            return new TestItemDTO
            {
                Id = test.Id.ToString().ToLower(),
                Name = test.Name,
                ImageUrl = test.ImageUrl,
                NumberOfAttempts = test.NumberOfAttempts,
                NumberOfQuestions = test.NumberOfQuestions,
                IsPrivate = test.IsPrivate,
                Description = test.Description
            };
        }

        public static TestInfoDTO MapToInfo(Test test)
        {
            return new TestInfoDTO
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
                NumberOfQuestions = test.NumberOfQuestions,
                IsPrivate = test.IsPrivate
            };
        }

        public static TestDTO MapToContent(Test test, List<Question> questions)
        {
            return new TestDTO
            {

                Name = test.Name,
                Duration = test.Duration,
                Questions = questions.Select(x => new QuestionDTO
                {
                    Id = x.Id.ToString(),
                    Content = x.Content,
                    ImageUrl = x.ImageUrl,
                    AnswerA = x.AnswerA,
                    AnswerB = x.AnswerB,
                    AnswerC = x.AnswerC,
                    AnswerD = x.AnswerD
                }).ToList()
            };
        }

        
    }
}
