using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Management;
using Core.Mappers;
using Data.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Management
{
    public class RoleManagementService : RoleMapper, IRoleManagementService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleManagementService( RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<APIResult> Add(CreateRoleDTO dto)
        {
            if (await _roleManager.RoleExistsAsync(dto.Name))
            {
                throw new ErrorException("Tên quyền đã tồn tại");
            }

            var result = await _roleManager.CreateAsync(new AppRole
            {
                Name = dto.Name,
                Description = dto.Description
            });

            if (result.Succeeded)
            {
                return new APISuccessResult("Tạo quyền thành công");
            }
            throw new ErrorException("Tạo quyền thất bại", result.Errors.Select(e => e.Description));
        }

        public async Task<APIResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId) 
                ?? throw new ErrorException("Quyền không tồn tại");

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return new APISuccessResult("Xóa quyền thành công");
            }
            throw new ErrorException("Xóa quyền thất bại", result.Errors.Select(e => e.Description));
        }

        public async Task<APIResult<IEnumerable<RoleDTO>>> GetAll()
        {
            var roles = await _roleManager.Roles.ToArrayAsync();
            return new APISuccessResult<IEnumerable<RoleDTO>>(roles.Select(e => Map(e)));
        }

        public async Task<APIResult> Update(RoleDTO dto)
        {
            var role = await _roleManager.FindByIdAsync(dto.Id)
                ?? throw new ErrorException("Quyền không tồn tại");

            if(await _roleManager.RoleExistsAsync(dto.Name))
            {
                throw new ErrorException("Tên quyền đã tồn tại");
            }

            role.Name = dto.Name;
            role.Description = dto.Description;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return new APISuccessResult("Cập nhật quyền thành công");
            }
            throw new ErrorException("Cập nhật thất bại", result.Errors.Select(e => e.Description));
        }
    }
}
