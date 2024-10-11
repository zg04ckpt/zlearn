
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Exceptions;
using ViewModels.System.Roles;

namespace Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleModel>> GetAll()
        {
            return await _roleManager.Roles
                .Select(r => new RoleModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name, 
                    Description = r.Description
                })
                .ToListAsync();
        }
        public async Task Add(string name, string description)
        {
            if (await _roleManager.RoleExistsAsync(name))
                throw new BadRequestException("Tên vai trò đã tồn tại");

            await _roleManager.CreateAsync(new AppRole
            {
                Name = name,
                Description = description
            });
        }
        public async Task Update(RoleModel role)
        {
            var oldRole = await _roleManager.FindByIdAsync(role.Id);
            if (oldRole == null)
                throw new NotFoundException("Vai trò không tồn tại");

            if (oldRole.Name.ToLower() != role.Name.ToLower() && (await _roleManager.RoleExistsAsync(role.Name)))
                throw new BadRequestException("Tên vai trò đã tồn tại");

            oldRole.Name = role.Name;
            oldRole.Description = role.Description;
            await _roleManager.UpdateAsync(oldRole);
        }
        public async Task Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new NotFoundException("Vai trò không tồn tại");

            await _roleManager.DeleteAsync(role);
        }

        
    }
}
