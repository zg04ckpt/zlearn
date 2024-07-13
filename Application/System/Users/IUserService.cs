using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Users;

namespace Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult> Authenticate(LoginRequest request);
        Task<ApiResult> Register(RegisterRequest request, string origin);
        Task<ApiResult> Logout();
        Task<ApiResult> RefreshToken(Token token);
        Task<ApiResult> EmailValidate(string userId, string token);
        Task<ApiResult> GetUsers(PagingRequest request);
        Task<ApiResult> GetUserById(string id);
        Task<ApiResult> UpdateUser(string id, UserUpdateRequest request);
        Task<ApiResult> GetAllRoles(string userId);
        Task<ApiResult> RoleAssign(string userId, RoleAssignRequest request);
    }
}
