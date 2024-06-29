using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        
        public async Task<ApiResult> Add(string roleName, string desc)
        {
            try
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                    return new ApiResult("Tên vai trò đã tồn tại", HttpStatusCode.BadRequest);

                await _roleManager.CreateAsync(new AppRole
                {
                    Name = roleName,
                    Description = desc
                });
                return new ApiResult();
            }
            catch(Exception ex) 
            {
                return new ApiResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult> Delete(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return new ApiResult("Vai trò không tồn tại", HttpStatusCode.NotFound);

                await _roleManager.DeleteAsync(role);
                return new ApiResult();
            }
            catch (Exception ex)
            {
                return new ApiResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult> GetAll()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return new ApiResult(roles);
        }

        public async Task<ApiResult> Update(string id, string newRoleName, string newDesc)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if(role == null) 
                    return new ApiResult("Vai trò không tồn tại", HttpStatusCode.NotFound);

                if (role.Name.ToLower()!=newRoleName.ToLower() && (await _roleManager.RoleExistsAsync(newRoleName)))
                    return new ApiResult("Tên vai trò đã tồn tại", HttpStatusCode.BadRequest);

                role.Name = newRoleName;
                role.Description = newDesc;
                await _roleManager.UpdateAsync(role);
                return new ApiResult();
            }
            catch (Exception ex)
            {
                return new ApiResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
