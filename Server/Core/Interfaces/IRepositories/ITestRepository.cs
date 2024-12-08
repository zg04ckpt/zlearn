using Core.Common;
using Core.DTOs;
using Data.Entities.TestEntities;
using System.Security.Claims;

namespace Core.Interfaces.IRepositories
{
    public interface ITestRepository : IBaseRepository<Test, Guid>
    {
        // -------------------- test ---------------------------
        //Task<PaginatedResult<TestItemDTO>> GetTestsAsListItem(int pageIndex, int pageSize, List<ExpressionFilter> filters, string? key);
        //Task<IEnumerable<TestInfoDTO>> GetTestInfosOfUser(string userId);
        //Task<TestInfoDTO> GetTestInfo(string testId);
        //Task<TestDTO> GetTestContent(ClaimsPrincipal user, string testId);
        //Task<UpdateTestDTO> GetUpdateTestContent(string userId, string testId);

        Task<List<Question>> GetQuestions(string testId);
        void RemoveQuestion(Question question);
        void UpdateQuestion(Question question);
        void AddQuestion(Question question);

        // ------------------ test result ---------------------
        Task<PaginatedResult<TestResult>> GetAllResults(int pageIndex, int pageSize, List<ExpressionFilter> filters);
        Task<List<TestResult>> GetResultsByUserId(string userId);
        void SaveResult(TestResult testResult);

        // ------------------ save test ---------------------
        Task<List<Test>> GetSavedTestsOfUser(string userId);
        void SaveTest(SavedTest savedTest);
        Task UnSave(string userId, string testId);
        Task<bool> IsSaved(string userId, string testId);

        // ------------------ Other ---------------------
        Task<List<Test>> GetTopByAttempt(int amount);
    }
}
