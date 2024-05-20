using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System;

namespace Application.System
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<ApiResult> Authenticate(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                    return new ApiResult("Tài khoản không tồn tại", HttpStatusCode.BadRequest);
                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.Remember, false);
                if (!result.Succeeded)
                    return new ApiResult("Mật khẩu không đúng", HttpStatusCode.BadRequest);

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, string.Join(";", roles))
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _config["Tokens:Issuer"],
                    audience: _config["Tokens:Issuer"],
                    claims: claims,
                    signingCredentials: creds,
                    expires: DateTime.Now.AddSeconds(30)
                );

                var resultToken = new JwtSecurityTokenHandler().WriteToken(token);
                return new ApiResult(new
                {
                    Token = resultToken,
                });
            }
            catch (Exception ex)
            {
                return new ApiResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
