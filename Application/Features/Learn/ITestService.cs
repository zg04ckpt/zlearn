using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<TestDetailResponse> GetDetailById(string id);
        Task<TestResponse> GetTestContentById(string id);
        Task Create(CreateTestRequest request);
        Task Update(string id, TestUpdateRequest request);
        Task Delete(string id);
        Task<PagingResponse<TestResult>> GetAllResults(PagingRequest request);
        Task<TestResultResponse> MarkTest(MarkTestRequest request);
        Task RemoveAllResults();
    }
}
