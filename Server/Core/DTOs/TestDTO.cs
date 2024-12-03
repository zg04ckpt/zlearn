using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    #region test
    public class TestDTO
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }

    public class TestItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberOfAttempts { get; set; }
        public bool IsPrivate { get; set; }
        public string Description { get; set; }
    }

    public class TestInfoDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string UpdatedDate { get; set; }
        public string CreatedDate { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string Source { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberOfAttempts { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class CreateTestDTO
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int Duration { get; set; }
        public string CategorySlug { get; set; }
        public bool IsPrivate { get; set; }
        public List<CreateQuestionDTO> Questions { get; set; }
    }

    public class UpdateTestDTO
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int Duration { get; set; }
        public string CategorySlug { get; set; }
        public bool IsPrivate { get; set; }
        public List<UpdateQuestionDTO> Questions { get; set; }
    }

    public class TestSearchDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CategorySlug { get; set; }
    }
    #endregion



    #region question
    public class QuestionDTO
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
    }

    public class CreateQuestionDTO
    {
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string? AnswerC { get; set; }
        public string? AnswerD { get; set; }
        public int CorrectAnswer { get; set; }
    }

    public class UpdateQuestionDTO
    {
        public string? Id { get; set; }
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string? AnswerC { get; set; }
        public string? AnswerD { get; set; }
        public int CorrectAnswer { get; set; }
    }
    #endregion



    #region result, save, mark test
    public class SaveTestDTO
    {
        public string UserId { get; set; }
        public string TestId { get; set; }
    }

    public class MarkTestDTO
    {
        public List<QuestionAnswerDTO> Answers { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TestId { get; set; }
        public string TestName { get; set; }
    }

    public class QuestionAnswerDTO
    {
        public string Id { get; set; }
        public int Selected { get; set; }
    }

    public class TestResultDTO
    {
        public int Total { get; set; }
        public double Score { get; set; }
        public int Correct { get; set; }
        public int Unselected { get; set; }
        public int UsedTime { get; set; }
        public List<int> Detail { get; set; }
    }

    public class TestResultSearchDTO
    {
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? TestName { get; set; }
        public string? UserName { get; set; }
    }
    #endregion
}
