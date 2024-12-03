using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Management
{
    public interface IUserManagementService
    {
        Task<APIResult<PaginatedResult<UserManagementDTO>>> GetAllUsers(int pageSize, int pageIndex, List<ExpressionFilter> filters);
        Task<APIResult<UserManagementDTO>> GetUserById(string userId);
        Task<APIResult> DeleteUser(string userId);
        Task<APIResult> UpdateUser(UserManagementDTO dto);
        Task<APIResult<IEnumerable<string>>> GetAllRolesOfUser(string userId);
        Task<APIResult> AssignRole(string userId, RoleAssignmentDTO dto);
    }
}
