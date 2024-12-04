using Core.Common;
using Core.DTOs;
using Data.Entities;
using System.Security.Claims;

namespace Core.Interfaces.IServices.Features 
{ 
    public interface ITestService
    {
        // ---------- Test -----------
        Task<APIResult<PaginatedResult<TestItemDTO>>> GetTestsAsListItems(int pageSize, int pageIndex, List<ExpressionFilter> filters);
        Task<APIResult<PaginatedResult<TestItemDTO>>> SearchTest(int pageSize, int pageIndex, TestSearchDTO data);
        Task<APIResult<List<TestInfoDTO>>> GetTestInfosOfUser(ClaimsPrincipal claimsPrincipal);
        Task<APIResult<TestInfoDTO>> GetTestInfo(string testId);
        Task<APIResult<TestDTO>> GetTestContent(ClaimsPrincipal claimsPrincipal, string testId);
        Task<APIResult<UpdateTestDTO>> GetTestUpdateContent(ClaimsPrincipal claimsPrincipal, string testId);
        Task<APIResult> CreateTest(ClaimsPrincipal claimsPrincipal, CreateTestDTO dto);
        Task<APIResult> UpdateTest(ClaimsPrincipal claimsPrincipal, string testId, UpdateTestDTO dto);
        Task<APIResult> DeleteTest(ClaimsPrincipal claimsPrincipal, string testId);


        // ---------- Test result ------------
        Task<APIResult<PaginatedResult<TestResult>>> GetAllResults(int pageSize, int pageIndex, List<ExpressionFilter> filters);
        Task<APIResult<List<TestResult>>> GetTestResultsOfUser(ClaimsPrincipal claimsPrincipal);
        Task<APIResult<TestResultDTO>> MarkTest(ClaimsPrincipal claimsPrincipal, MarkTestDTO dto, string ip);


        // ---------- Save test --------------
        Task<APIResult> SaveTest(ClaimsPrincipal claimsPrincipal, string testId);
        Task<APIResult<List<TestItemDTO>>> GetSavedTestsOfUser(ClaimsPrincipal claimsPrincipal);
        Task<APIResult> DeleteFromSaved(ClaimsPrincipal claimsPrincipal, string testId);
        Task<APIResult<bool>> IsSaved(ClaimsPrincipal claimsPrincipal, string testId);
    }
}
