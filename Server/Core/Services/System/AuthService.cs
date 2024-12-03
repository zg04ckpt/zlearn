using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IServices;
using Core.Interfaces.IServices.Common;
using Core.Interfaces.IServices.System;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core.Services.System
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailSender;
        private readonly ISummaryService _summaryService;
        private readonly ILogger<AuthService> _logger;
        private readonly ILogService _logService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, IEmailService emailSender, ISummaryService summaryService, ILogger<AuthService> logger, ILogService logService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailSender = emailSender;
            _summaryService = summaryService;
            _logger = logger;
            _logService = logService;
        }

        public async Task<APIResult> ValidateEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ErrorException("Không tìm thấy user");

            if (await _userManager.IsEmailConfirmedAsync(user))
                throw new ErrorException("Email đã được xác thực");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var checkResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!checkResult.Succeeded)
                throw new ErrorException("Xác thực email không thành công");

            return new APISuccessResult("Xác thực email thành công");
        }

        public async Task<APIResult<LoginResponseDTO>> Login(LoginDTO request)
        {
            //check valid login
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new ErrorException("Tài khoản không tồn tại");

            //check email validation
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new ErrorException("Email chưa xác thực, vui lòng kiểm tra email đăng kí để lấy link xác thực");
            }
            
            //check pass
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.Remember, request.Remember);
            if (!result.Succeeded)
                throw new ErrorException("Mật khẩu không đúng");


            //get tokens
            var token = await GenerateToken(user);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(double.Parse(_config.GetSection(Consts.JWT_SECTION)["AccessLifeTime"]));
            await _userManager.UpdateAsync(user);

            return new APISuccessResult<LoginResponseDTO>(new LoginResponseDTO
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                FullName = user.FirstName != null && user.LastName != null ? user.LastName + " " + user.FirstName : "",
                Username = user.UserName,
                ProfileImage = user.ImageUrl,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                ExpirationTime = user.RefreshTokenExpiryTime
            });
        }

        public async Task<APIResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new APISuccessResult();
        }

         public async Task<APIResult<TokenDTO>> RefreshToken(TokenDTO token)
        {
            //validate access token
            var principal = GetClaimsPrincipalFromExpiredToken(token.AccessToken);
            if (principal == null)
                throw new ErrorException("Access token không hợp lệ");

            //validate refresh token
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != token.RefreshToken)
                throw new ErrorException("Refresh token không hợp lệ");

            //refresh token expired => login to generate new token
            if (user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new ErrorException("Phiên đăng nhập hết hạn");

            //update both access and refresh
            var newToken = await GenerateToken(user);
            user.RefreshToken = newToken.RefreshToken;
            await _userManager.UpdateAsync(user);
            return new APISuccessResult<TokenDTO>(newToken);
        }

        public async Task<APIResult> Register(RegisterDTO request, string origin)
        {
            //check if user existed
            if ((await _userManager.FindByEmailAsync(request.Email)) != null)
                throw new ErrorException("Email đã được sử dụng");

            //check if userName is used
            if ((await _userManager.FindByNameAsync(request.UserName)) != null)
                throw new ErrorException("Tên người dùng đã tồn tại");

            //create new user
            var newUser = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedDate = DateOnly.FromDateTime(DateTime.Today).ToString()
            };

            //save user to db
            var createProcess = await _userManager.CreateAsync(newUser, request.Password);
            if (!createProcess.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                throw new ErrorException("Tạo tài khoản thất bại!");
            }

            //assign default role
            var roleAssignResult = await _userManager.AddToRoleAsync(newUser, Consts.USER_ROLE);
            if(!roleAssignResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                throw new ErrorException("Lỗi gán quyền người dùng");
            }

            //get url contains a code to validate
            var validateCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            validateCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(validateCode));
            var url = $"{origin}/auth/email-confirm?id={newUser.Id}&token={validateCode}";

            //get the template of confirm account email
            string? emailContent = null;
            try
            {
                var path = Path.Combine(AppContext.BaseDirectory, "Resources", "Templates", "RegisterEmail.html");
                emailContent = await File.ReadAllTextAsync(path);
                emailContent = emailContent.Replace("[confirm-url]", url);
            }
            catch(Exception ex)
            {
                await _userManager.DeleteAsync(newUser);
                throw new ErrorException("Lỗi đọc file templates");
            }

            //send mail
            var sendMailResult = await _emailSender.SendTo(
                newUser.Email,
                "Xác thực email của bạn",
                emailContent!
            );
            if (!sendMailResult)
            {
                await _userManager.DeleteAsync(newUser);
                throw new ErrorException("Lỗi gửi mail xác thực");
            }

            //summary
            await _summaryService.IncreaseUserCount();
            

            //log
            _logger.LogInformation($"Người dùng đăng kí mới: {newUser.LastName} {newUser.FirstName}");
            await _logService.SendInfoLog($"Người dùng đăng kí mới: {newUser.LastName} {newUser.FirstName}");

            return new APISuccessResult("Đăng kí tài khoản thành công");
        }

        private async Task<TokenDTO> GenerateToken(AppUser user)
        {
            //generate access token
            var roles = await _userManager.GetRolesAsync(user);
            var publicClaims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.GivenName, user.LastName + " " + user.FirstName),
                new (ClaimTypes.Name, user.UserName), //require for refresh
                new (ClaimTypes.Email, user.Email),

            };
            foreach (var role in roles)
            {
                publicClaims.Add(new(ClaimTypes.Role, role));
            }
            var secretKey = Environment.GetEnvironmentVariable(Consts.SECRET_KEY);
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var token = new JwtSecurityToken
            (
                issuer: _config.GetSection(Consts.JWT_SECTION)["Issuer"],
                audience: _config.GetSection(Consts.JWT_SECTION)["Audience"],
                claims: publicClaims,
                signingCredentials: new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256),
                expires: DateTime.Now.AddMinutes(double.Parse(_config.GetSection(Consts.JWT_SECTION)["RefreshLifeTime"]))
            );
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var accessToken = jwtTokenHandler.WriteToken(token);

            //generate refresh token
            string refreshToken;
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                refreshToken = Convert.ToBase64String(randomBytes);
            }

            return new TokenDTO
            {
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

                ValidIssuer = _config.GetSection(Consts.JWT_SECTION)["Issuer"],
                ValidAudience = _config.GetSection(Consts.JWT_SECTION)["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Consts.SECRET_KEY)))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }

        public async Task<APIResult> ForgotPassword(ForgotPasswordDTO data)
        {
            var user = await _userManager.FindByEmailAsync(data.Email)
                ?? throw new ErrorException("Tài khoản không tồn tại");

            //generate url contains token for reset password
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config[Consts.SettingKeys.JWT_AUDIENCE]}/auth/reset-password?id={user.Id}&token={token}";

            //get email template
            var path = Path.Combine(AppContext.BaseDirectory, "Resources", "Templates", "ResetPasswordEmail.html");
            var template = await File.ReadAllTextAsync(path);
            template = template.Replace("[username]", user.UserName);
            template = template.Replace("[reset-pass-url]", url);

            //send mail
            var sendMailResult = await _emailSender.SendTo(
                    user.Email,
                    "Tạo lại mật khẩu mới - ZLEARN",
                    template
                );
            if(!sendMailResult)
            {
                return new APIErrorResult("Gửi mail thất bại, yêu cầu tạo mật khẩu mới không thành công!");
            }
            return new APISuccessResult("Gửi yêu cầu thành công, vui lòng kiểm tra email của bạn!");
        }

        public async Task<APIResult> ResetPassword(ResetPasswordDTO data)
        {
            var user = await _userManager.FindByIdAsync(data.UserId)
               ?? throw new ErrorException("Tài khoản không tồn tại");

            // tạo lại mật khẩu
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(data.Token));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, decodedToken, data.Password);
            if(!resetPassResult.Succeeded)
            {
                return new APIErrorResult("Cập nhật mật khẩu mới thất bại!");
            }

            return new APISuccessResult("Cập nhật mật khẩu mới thành công!");
        }
    }
}
