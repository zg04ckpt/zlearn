using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Users;

namespace Application.System.Users
{
    public interface IUserService
    {
        Task<List<AppUser>> GetUsers(PagingRequest request);
        Task<AppUser> GetUserById(string id);
        Task<UserDetailModel> GetUserDetail(string id);
        Task UpdateUserDetail(string id, UserDetailModel request);
        Task<List<string>> GetAllRoles(string userId);
        Task RoleAssign(string userId, RoleAssignRequest request);
    }
}
