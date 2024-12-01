using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.System
{
    public interface IUserService
    {
        Task<APIResult<UserProfileDTO>> GetMyProfile(ClaimsPrincipal claimsPrincipal);
        Task<APIResult> UpdateMyProfile(ClaimsPrincipal claimsPrincipal, UserProfileDTO dto);
        Task<APIResult<UserInfoDTO>> GetOtherUserProfile(ClaimsPrincipal claimsPrincipal, string userId);
        Task<APIResult> Like(ClaimsPrincipal claimsPrincipal, string likedUserId);
        Task<APIResult<bool>> IsLiked(ClaimsPrincipal claimsPrincipal, string likedUserId);
    }
}
