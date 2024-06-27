using Data.Entities;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System;

namespace Application.System
{   
    public interface IUserService
    {
        Task<ApiResult> Authenticate(LoginRequest request);
        Task<ApiResult> Register(RegisterRequest request, string host, string scheme);
        Task<ApiResult> RefreshToken(Token token);
        Task<ApiResult> EmailValidate(string userId, string token);
    }
}
