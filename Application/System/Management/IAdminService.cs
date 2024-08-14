using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Manage;

namespace Application.System.Manage
{
    public interface IAdminService
    {
        Task<PagingResponse<UserManagementModel>> GetAllUsers(PagingRequest request);
        Task<UserManagementModel> GetUserById(string id);
        Task DeleteUser(string id);
        Task UpdateUser(UserManagementModel user);
        Task<List<string>> GetAllRolesOfUser(string userId);
        Task AssignRole(string userId, RoleAssignRequest request);
        Task<PagingResponse<UserManagementModel>> FindByUserName(string key, PagingRequest request);
        Task<PagingResponse<UserManagementModel>> FindByName(string key, PagingRequest request);
        Task<PagingResponse<UserManagementModel>> FindByEmail(string key, PagingRequest request);
        Task<PagingResponse<UserManagementModel>> FindByRole(string key, PagingRequest request);
        Task<PagingResponse<UserManagementModel>> FindByPhoneNum(string key, PagingRequest request);
    }
}
