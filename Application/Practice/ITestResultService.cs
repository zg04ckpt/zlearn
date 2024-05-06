using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Test;

namespace Application.Practice
{
    public interface ITestResultService
    {
        Task<ApiResult> GetAll();
        Task<ApiResult> Create(TestResultCreateRequest request);
    }
}
