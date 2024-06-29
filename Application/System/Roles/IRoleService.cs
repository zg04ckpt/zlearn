using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;

namespace Application.System.Roles
{
    public interface IRoleService
    {
        Task<ApiResult> GetAll();
        Task<ApiResult> Add(string roleName, string desc);
        Task<ApiResult> Update(string id, string newRoleName, string newDesc);
        Task<ApiResult> Delete(string roleName);
    }
}
