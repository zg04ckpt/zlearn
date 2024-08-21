using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Question;
using ViewModels.QuestionSet;
using ViewModels.Test;

namespace Application.Practice
{
    public interface ITestService
    {
        Task<List<TestResponse>> GetAll();
        Task<TestResponse> GetById(string id);
        Task<List<QuestionViewModel>> GetAllQuestionByTestId(string id);
        Task Create(CreateTestRequest request);
        Task Update(string id, TestUpdateRequest request);
        Task Delete(string id);
        Task<List<TestResultResponse>> GetAllResults();
        Task SaveResult(SaveTestResultRequest request, ConnectionInfo connectionInfo);
        Task RemoveAllResults();
    }
}
