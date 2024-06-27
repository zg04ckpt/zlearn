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
using Utilities;
using Application.Common;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;

namespace Application.System
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailSender = emailSender;
        }

        public async Task<ApiResult> Authenticate(LoginRequest request)
        {
            try
            {
                //check valid login
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                    return new ApiResult("Tài khoản không tồn tại", HttpStatusCode.BadRequest);
                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.Remember, false);
                if (result.IsNotAllowed)
                    return new ApiResult("Email chưa xác thực, vui lòng kiểm tra email đăng kí để lấy link xác thực", HttpStatusCode.BadRequest);
                if (!result.Succeeded)
                    return new ApiResult("Mật khẩu không đúng", HttpStatusCode.BadRequest);

                var token = await GenerateToken(user);
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(double.Parse(_config[Consts.AppSettingsKey.REFRESH_LIFE_TIME]));

                await _userManager.UpdateAsync(user);
                return new ApiResult(token);
            }
            catch (Exception ex)
            {
                return new ApiResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ApiResult> Register(RegisterRequest request, string host, string scheme)
        {
            try
            {
                //check user existed
                if (await _userManager.FindByEmailAsync(request.Email) != null)
                    return new ApiResult("Email existed!", HttpStatusCode.BadRequest);
                
                //create new user
                var newUser = new AppUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Gender = request.Gender,
                    DateOfBirth = request.DateOfBirth
                };

                //save user to db
                var createProcess = await _userManager.CreateAsync(newUser, request.Password);
                if(createProcess.Succeeded)
                {
                    if((await _userManager.AddToRoleAsync(newUser, Consts.DEFAULT_USER_ROLE)).Succeeded)
                    {
                        //email confirm
                        var validateCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        validateCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(validateCode));
                        var url = $"{scheme}://{host}/api/users/email-confirm?id={newUser.Id}&token={validateCode}";
                        var sendMailResult = await _emailSender.SendTo(
                            newUser.Email, 
                            "Xác thực email của bạn", 
                            $"Bạn đã hoàn tất đăng kí tài khoản tại zg04, nhất vào <a href=\"{url}\">đây</a> để xác thực email của bạn."
                        );
                        if(sendMailResult)
                            return new ApiResult();
                        else
                            return new ApiResult("Lỗi gửi mail xác thực", HttpStatusCode.InternalServerError);
                    }
                    else
                        return new ApiResult("Lỗi gán quyền user!", HttpStatusCode.InternalServerError);
                }
                return new ApiResult("Đăng kí không thành công!", HttpStatusCode.BadRequest);
            }
            catch(Exception ex)
            {
                return new ApiResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ApiResult> EmailValidate(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return new ApiResult("Không tìm thấy user", HttpStatusCode.BadRequest);

                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
                var checkResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
                if (checkResult.Succeeded)
                    return new ApiResult();
                return new ApiResult("Xác thực không thành công", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return new ApiResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ApiResult> RefreshToken(Token token)
        {
            try
            {
                //validate access token
                var principal = GetClaimsPrincipalFromExpiredToken(token.AccessToken);
                if (principal is null)
                    return new ApiResult("Invalid access token", HttpStatusCode.BadRequest);

                //validate refresh token
                var user = await _userManager.FindByNameAsync(principal.Identity.Name);
                if (user is null || user.RefreshToken != token.RefreshToken)
                    return new ApiResult("Invalid refresh token", HttpStatusCode.BadRequest);

                //refresh token expired => login to generate new token
                if(user.RefreshTokenExpiryTime <= DateTime.Now)
                    return new ApiResult("Refresh token expired", HttpStatusCode.Unauthorized);

                //refresh token didn't expired => update both access and refresh
                var newToken = await GenerateToken(user);
                user.RefreshToken = newToken.RefreshToken;
                await _userManager.UpdateAsync(user);
                return new ApiResult(newToken);
            }
            catch (Exception ex)
            {
                return new ApiResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
        private async Task<Token> GenerateToken(AppUser user)
        {
            //generate access token
            var roles = await _userManager.GetRolesAsync(user);
            var publicClaims = new Claim[]
            {
                new (ClaimTypes.Name, user.UserName), //require for refresh
                new (ClaimTypes.GivenName, user.FirstName),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Role, string.Join(',', roles))
            };
            var secretKey = Environment.GetEnvironmentVariable(Consts.EnvKey.SECRET_KEY);
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var token = new JwtSecurityToken
            (
                issuer: _config[Consts.AppSettingsKey.ISSUER],
                audience: _config[Consts.AppSettingsKey.AUDIENCE],
                claims: publicClaims,
                signingCredentials: new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256),
                expires: DateTime.Now.AddMinutes(double.Parse(_config[Consts.AppSettingsKey.ACCESS_LIFE_TIME]))
            );
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var accessToken = jwtTokenHandler.WriteToken(token);

            //generate refresh token
            string refreshToken;
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                refreshToken =  Convert.ToBase64String(randomBytes);
            }  
            
            return new Token { 
                AccessToken = accessToken,
                RefreshToken = refreshToken 
            };
        }
        private ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string expiredToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,

                ValidIssuer = _config[Consts.AppSettingsKey.ISSUER],
                ValidAudience = _config[Consts.AppSettingsKey.AUDIENCE],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Consts.EnvKey.SECRET_KEY)))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if(jwtSecurityToken is null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }    

            return principal;
        }
        
    }
}
