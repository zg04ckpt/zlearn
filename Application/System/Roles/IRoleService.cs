using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Roles;

namespace Application.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleModel>> GetAll();
        Task Add(string name, string description);
        Task Update(RoleModel role);
        Task Delete(string roleName);
    }
}
