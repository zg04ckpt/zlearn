using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.System
{
    public interface IAuthService
    {
        Task<APIResult<LoginResponseDTO>> Login(LoginDTO request);
        Task<APIResult> Register(RegisterDTO request, string origin);
        Task<APIResult> Logout();
        Task<APIResult<TokenDTO>> RefreshToken(TokenDTO token);
        Task<APIResult> ValidateEmail(string userId, string token);
    }
}
