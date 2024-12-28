using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Management;
using Core.Mappers;
using Data.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Management
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserManagementService(UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<APIResult<List<UserFindDataDTO>>> GetFindData(ClaimsPrincipal claims)
        {
            if (!claims.IsInRole("Admin"))
            {
                throw new ForbiddenException();
            }

            var users = await _userRepository.GetAll();
            return new APISuccessResult<List<UserFindDataDTO>>(users.Select(e => UserMapper.MapToFindData(e)).ToList()
            );
        }
        public async Task<APIResult> AssignRole(string userId, RoleAssignmentDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ErrorException("Không tìm thấy user");

            var userRoles = new HashSet<string>(await _userManager.GetRolesAsync(user));
            var rolesToAdd = new List<string>();
            var rolesToRemove = new List<string>();
            foreach (var role in dto.Roles)
            {
                if (role.Selected && !userRoles.Contains(role.Name))
                {
                    rolesToAdd.Add(role.Name);
                }
                else if (!role.Selected && userRoles.Contains(role.Name))
                {
                    rolesToRemove.Add(role.Name);
                }
            }

            if (rolesToRemove.Count > 0)
            {
                var result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if(!result.Succeeded)
                {
                    throw new ErrorException("Gán quyền thất bại", result.Errors.Select(e => e.Description));
                }
            }
            if (rolesToAdd.Count > 0)
            {
                var result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!result.Succeeded)
                {
                    throw new ErrorException("Gán quyền thất bại", result.Errors.Select(e => e.Description));
                }
            }
            return new APISuccessResult("Gán quyền thành công");
        }

        public async Task<APIResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ErrorException("Không tìm thấy user");

            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded)
            {
                return new APISuccessResult("Xóa user thành công");
            }
            else
            {
                throw new ErrorException("Xóa user thất bại", result.Errors.Select(e => e.Description));
            }
        }

        public async Task<APIResult<IEnumerable<string>>> GetAllRolesOfUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ErrorException("Không tìm thấy user");

            var roles = await _userManager.GetRolesAsync(user);

            return new APISuccessResult<IEnumerable<string>>(roles);
        }

        public async Task<APIResult<PaginatedResult<UserManagementDTO>>> GetAllUsers(UserSearchDTO data)
        {
            var query = _userRepository.GetQuery().AsNoTracking();

            //filter
            if (!string.IsNullOrEmpty(data.UserName))
            {
                query = query.Where(e => e.UserName.Equals(data.UserName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(data.FirstName))
            {
                query = query.Where(e => e.FirstName.Equals(data.FirstName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(data.LastName))
            {
                query = query.Where(e => e.LastName.Equals(data.LastName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(data.Email))
            {
                query = query.Where(e => e.Email.Equals(data.Email, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(data.CreatedDate))
            {
                query = query.Where(e => DateTime.Parse(e.CreatedDate).CompareTo(DateTime.Parse(data.CreatedDate)) == 0);
            }

            //paging
            query = query
                .Skip((data.PageIndex - 1) * data.PageSize)
                .Take(data.PageSize)
                .OrderBy(e => e.CreatedDate);

            var res = new List<UserManagementDTO>();
            var users = await query.ToListAsync();
            foreach ( var user in users )
            {
                var dto = UserMapper.MapToManage(user);
                dto.Roles = await _userManager.GetRolesAsync(user);
                res.Add(dto);
            }    

            return new APISuccessResult<PaginatedResult<UserManagementDTO>>(new PaginatedResult<UserManagementDTO>
            {
                Total = await query.CountAsync(),
                Data = res
            });
        }

        public async Task<APIResult<UserManagementDTO>> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ErrorException("Không tìm thấy user");

            var dto = UserMapper.MapToManage(user);
            dto.Roles = await _userManager.GetRolesAsync(user);
            return new APISuccessResult<UserManagementDTO>(dto);
        }

        public async Task<APIResult> UpdateUser(UserManagementDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id)
                ?? throw new ErrorException("Không tìm thấy user");

            user = UserMapper.MapFromManage(user, dto);
            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded)
            {
                return new APISuccessResult("Cập nhật thành công");
            }
            else
            {
                throw new ErrorException("Cập nhật thất bại: " + string.Join(";", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
