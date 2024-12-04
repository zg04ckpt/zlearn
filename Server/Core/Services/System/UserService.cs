using Core.Common;
using Core.DTOs;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Common;
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
using static System.Net.Mime.MediaTypeNames;

namespace Core.Services.System
{
    public class UserService : IUserService
    {
        private const string IMAGE_REQUEST_PATH = "/api/images/user/";

        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly string _imageFolderPath;
        private readonly IFileService _fileService;


        public UserService(UserManager<AppUser> userManager, IUserRepository userRepository, IFileService fileService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _imageFolderPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "User");
            _fileService = fileService;
        }

        public async Task<APIResult<UserProfileDTO>> GetMyProfile(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new APISuccessResult<UserProfileDTO>(UserMapper.MapToProfile(user));
        }

        public async Task<APIResult<UserInfoDTO>> GetOtherUserProfile(ClaimsPrincipal claimsPrincipal, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userInfo = UserMapper.MapToInfo(user);
            if(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                string currentUserId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                userInfo.IsLiked = await _userRepository.IsLiked(currentUserId, userId);
            }
            else
            {
                userInfo.IsLiked = false;
            }
            return new APISuccessResult<UserInfoDTO>(userInfo);
        }

        public async Task<APIResult<bool>> IsLiked(ClaimsPrincipal claimsPrincipal, string likedUserId)
        {
            string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return new APISuccessResult<bool>(await _userRepository.IsLiked(userId, likedUserId));
        }

        public async Task<APIResult> Like(ClaimsPrincipal claimsPrincipal, string likedUserId)
        {
            string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            if(!await _userRepository.IsLiked(userId, likedUserId))
            {
                await _userRepository.Like(userId, likedUserId);
                return new APISuccessResult("Đã like!");
            }
            return new APIErrorResult("Mỗi user chỉ được like một lần với một người");
        }

        public async Task<APIResult> UpdateMyProfile(ClaimsPrincipal claimsPrincipal, UserProfileDTO dto)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);
            user = UserMapper.MapFromProfile(user, dto);
            if (dto.Image != null)
            {
                if (user.ImageUrl != null)
                {
                    await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(user.ImageUrl)));
                }
                user.ImageUrl = IMAGE_REQUEST_PATH + await _fileService.Save(dto.Image, _imageFolderPath);
            }
            await _userManager.UpdateAsync(user);
            return new APISuccessResult("Cập nhật thành công");
        }
    }
}
