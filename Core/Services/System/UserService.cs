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

        public async Task<APIResult<UserProfileDTO>> GetProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new APISuccessResult<UserProfileDTO>(UserMapper.MapToProfile(user));
        }

        public async Task<APIResult> UpdateProfile(string userId, UserProfileDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.UpdateAsync(UserMapper.MapFromProfile(user, dto));
            return new APISuccessResult("Cập nhật thành công");
        }
    }
}
