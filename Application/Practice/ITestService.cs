using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.QuestionSet;

namespace Application.Practice
{
    public interface ITestService
    {
        Task<ApiResult> GetAll();
        Task<ApiResult> GetById(string id);
        Task<ApiResult> Create(CreateTestRequest request);
        Task<ApiResult> Update(string id, TestUpdateRequest request);
        Task<ApiResult> Delete(string id);
    }
}
