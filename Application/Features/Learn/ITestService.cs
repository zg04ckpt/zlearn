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
        Task<List<TestResponse>> GetAll();
        Task<TestResponse> GetById(string id);
        Task<List<QuestionModel>> GetAllQuestionByTestId(string id);
        Task Create(CreateTestRequest request);
        Task Update(string id, TestUpdateRequest request);
        Task Delete(string id);
        Task<List<TestResultResponse>> GetAllResults();
        Task SaveResult(SaveTestResultRequest request);
        Task RemoveAllResults();
    }
}
