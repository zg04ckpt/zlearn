using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Features.Learn.Test;
using ViewModels.Features.Learn.Test.Question;

namespace Application.Features.Learn
{
    public interface ITestService
    {
        Task<PagingResponse<TestItem>> GetAll(PagingRequest request);
        Task<List<TestDetailResponse>> GetTestsByUserId(string userId);
        Task<TestDetailResponse> GetDetailById(string id);
        Task<TestResponse> GetTestContentById(ClaimsPrincipal user, string testId);

        //------------------------
        Task<TestResultResponse> MarkTest(MarkTestRequest request, string ip, ClaimsPrincipal user);
        Task<PagingResponse<TestResult>> GetAllResults(string userId, PagingRequest request);
        Task RemoveAllResults();

        //------------------------

        Task Create(CreateTestRequest request, ClaimsPrincipal user);
        Task Update(string userId, string testId, TestUpdateRequest request);
        Task<TestUpdateContent> GetTestUpdateContent(string userId, string testId);
        Task Delete(string testId, string userId);

        //------------------------
        Task SaveTest(string userId, string testId);
        Task<List<TestItem>> GetSavedTestsByUserId(string userId);
        Task DeleteFromSaved(string userId, string testId);
        Task<bool> IsSaved(string userId, string testId);
    }
}
