using Data.Entities;
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
        Task<List<AppRole>> GetAll();
        Task Add(string roleName, string desc);
        Task Update(string id, string newRoleName, string newDesc);
        Task Delete(string roleName);
    }
}
