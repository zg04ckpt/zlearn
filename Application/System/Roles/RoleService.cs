using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using ViewModels.Common;

namespace Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<AppRole>> GetAll()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task Add(string roleName, string desc)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                throw new BadRequestException("Tên vai trò đã tồn tại");

            await _roleManager.CreateAsync(new AppRole
            {
                Name = roleName,
                Description = desc
            });
        }
        public async Task Update(string id, string newRoleName, string newDesc)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new NotFoundException("Vai trò không tồn tại");

            if (role.Name.ToLower() != newRoleName.ToLower() && (await _roleManager.RoleExistsAsync(newRoleName)))
                throw new BadRequestException("Tên vai trò đã tồn tại");

            role.Name = newRoleName;
            role.Description = newDesc;
            await _roleManager.UpdateAsync(role);
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
