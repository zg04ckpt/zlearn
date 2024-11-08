using Core.Common;
using Core.DTOs;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.System;
using Core.Mappers;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.System
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<APIResult<UserProfileDTO>> GetMyProfile(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new APISuccessResult<UserProfileDTO>(UserMapper.MapToProfile(user));
        }

        public async Task<APIResult<UserInfoDTO>> GetOtherUserProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new APISuccessResult<UserInfoDTO>(UserMapper.MapToInfo(user));
        }

        public async Task<APIResult> UpdateMyProfile(ClaimsPrincipal claimsPrincipal, UserProfileDTO dto)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.UpdateAsync(UserMapper.MapFromProfile(user, dto));
            return new APISuccessResult("Cập nhật thành công");
        }
    }
}
