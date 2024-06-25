using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using ViewModels.Common;
using ViewModels.System;

namespace Application.System
{   
    public interface IUserService
    {
        Task<ApiResult> Authenticate(LoginRequest request);
        Task<ApiResult> Register(RegisterRequest request, string host, string scheme);
        Task<ApiResult> EmailValidate(string userId, string token);
    }
}
