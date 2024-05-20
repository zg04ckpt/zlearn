using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Test;

namespace Application.Practice
{
    public interface ITestResultService
    {
        Task<ApiResult> GetAll();
        Task<ApiResult> Create(TestResultCreateRequest request, ConnectionInfo connectionInfo);
        Task<ApiResult> RemoveAll();
    }
}
