using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Management
{
    public interface IRoleManagementService
    {
        Task<APIResult<IEnumerable<RoleDTO>>> GetAll();
        Task<APIResult> Add(CreateRoleDTO dto);
        Task<APIResult> Update(RoleDTO dto);
        Task<APIResult> Delete(string roleId);
    }
}
