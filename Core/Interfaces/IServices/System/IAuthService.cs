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
        Task<APIResult<LoginResponseDTO>> Login(LoginDTO data);
        Task<APIResult> Register(RegisterDTO data, string origin);
        Task<APIResult> Logout();
        Task<APIResult> ForgotPassword(ForgotPasswordDTO data);
        Task<APIResult> ResetPassword(ResetPasswordDTO data);
        Task<APIResult<TokenDTO>> RefreshToken(TokenDTO token);
        Task<APIResult> ValidateEmail(string userId, string token);
    }
}
