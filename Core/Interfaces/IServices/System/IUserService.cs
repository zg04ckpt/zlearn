using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.System
{
    public interface IUserService
    {
        Task<APIResult<UserProfileDTO>> GetProfile(string userId);
        Task<APIResult<bool>> UpdateProfile(string userId, UserProfileDTO dto);
    }
}
