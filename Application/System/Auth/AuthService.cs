using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities;
using Data.Entities;
using Application.Common;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.RegularExpressions;
using ViewModels.System.Auth;

namespace Application.System.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailSender = emailSender;
        }

        public async Task EmailValidate(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Không tìm thấy user");

            if (await _userManager.IsEmailConfirmedAsync(user))
                throw new BadRequestException("Email đã được xác thực");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var checkResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!checkResult.Succeeded)
                throw new BadRequestException("Xác thực email không thành công");
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            //check valid login
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new NotFoundException("Tài khoản không tồn tại");

            //check email validation
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.Remember, request.Remember);
            if (result.IsNotAllowed)
                throw new BadRequestException("Email chưa xác thực, vui lòng kiểm tra email đăng kí để lấy link xác thực");

            //check pass
            if (!result.Succeeded)
                throw new BadRequestException("Mật khẩu không đúng");

            //gen tokens
            var token = await GenerateToken(user);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(double.Parse(_config[Consts.AppSettingsKey.REFRESH_LIFE_TIME]));
            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                ExpirationTime = user.RefreshTokenExpiryTime
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Token> RefreshToken(Token token)
        {
            //validate access token
            var principal = GetClaimsPrincipalFromExpiredToken(token.AccessToken);
            if (principal == null)
                throw new BadRequestException("Access token không hợp lệ");

            //validate refresh token
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != token.RefreshToken)
                throw new BadRequestException("Refresh token không hợp lệ");

            //refresh token expired => login to generate new token
            if (user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new BadRequestException("Phiên đăng nhập hết hạn");

            //update both access and refresh
            var newToken = await GenerateToken(user);
            user.RefreshToken = newToken.RefreshToken;
            await _userManager.UpdateAsync(user);
            return newToken;
        }
        public async Task Register(RegisterRequest request, string origin)
        {
            if (request.Password != request.ConfirmPassword)
                throw new BadRequestException("Mật khẩu xác nhận không khớp");

            //check if user existed
            if ((await _userManager.FindByEmailAsync(request.Email)) != null)
                throw new BadRequestException("Email đã được sử dụng");

            //check if userName is used
            if ((await _userManager.FindByNameAsync(request.UserName)) != null)
                throw new BadRequestException("Tên người dùng đã tồn tại");

            //check userName valid
            if(request.UserName.Length < 2 || request.UserName.Length > 8)
                throw new BadRequestException("Độ dài tên người dùng phải từ 2 đến 8 kí tự");

            const string userNameRegex = "^[a-zA-Z0-9]+$";
            if(!Regex.IsMatch(request.UserName, userNameRegex))
                throw new BadRequestException("Tên người dùng không chứa kí tự đặc biệt (kể cả có dấu)");

            //check pass valid
            if(request.Password.Length > 16 || request.Password.Length < 8)
                throw new BadRequestException("Độ dài mật khẩu phải từ 8 đến 16 kí tự");

            const string passwordRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.@!#*]).+$";
            if(!Regex.IsMatch(request.Password, passwordRegex))
                throw new BadRequestException("Mật khẩu phải chứa kí tự in thường, in hoa, chữ số và kí tự đặc biệt .@!#*");

            //create new user
            var newUser = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                CreatedDate = DateOnly.FromDateTime(DateTime.Today).ToString()
            };

            //save user to db
            var createProcess = await _userManager.CreateAsync(newUser, request.Password);
            if (createProcess.Succeeded)
            {
                var roleAssignResult = await _userManager.AddToRoleAsync(newUser, Consts.DEFAULT_USER_ROLE);
                if (roleAssignResult.Succeeded)
                {
                    //email confirm
                    var validateCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    validateCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(validateCode));
                    var url = $"{origin}/auth/email-confirm?id={newUser.Id}&token={validateCode}";
                    var sendMailResult = await _emailSender.SendTo(
                        newUser.Email,
                        "Xác thực email của bạn",
                        GetValidationEmailHtml(url)
                    );

                    if (!sendMailResult)
                    {
                        await _userManager.DeleteAsync(newUser);
                        throw new Exception("Lỗi gửi mail xác thực");
                    }
                }
                else
                {
                    await _userManager.DeleteAsync(newUser);
                    throw new Exception("Lỗi gán quyền user");
                }
            }
            else
            {
                await _userManager.DeleteAsync(newUser);
                throw new BadRequestException("Đăng kí không thành công!");
            }    
        }
        #region private
        private string GetValidationEmailHtml(string confirmUrl)
        {
            return $@"<!DOCTYPE html>
<html>
<head>

  <meta charset=""utf-8"">
  <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
  <title>Email Confirmation</title>
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
  <style type=""text/css"">
  /**
   * Google webfonts. Recommended to include the .woff version for cross-client compatibility.
   */
  @media screen {{
    @font-face {{
      font-family: 'Source Sans Pro';
      font-style: normal;
      font-weight: 400;
      src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
    }}
    @font-face {{
      font-family: 'Source Sans Pro';
      font-style: normal;
      font-weight: 700;
      src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
    }}
  }}
  /**
   * Avoid browser level font resizing.
   * 1. Windows Mobile
   * 2. iOS / OSX
   */
  body,
  table,
  td,
  a {{
    -ms-text-size-adjust: 100%; /* 1 */
    -webkit-text-size-adjust: 100%; /* 2 */
  }}
  /**
   * Remove extra space added to tables and cells in Outlook.
   */
  table,
  td {{
    mso-table-rspace: 0pt;
    mso-table-lspace: 0pt;
  }}
  /**
   * Better fluid images in Internet Explorer.
   */
  img {{
    -ms-interpolation-mode: bicubic;
  }}
  /**
   * Remove blue links for iOS devices.
   */
  a[x-apple-data-detectors] {{
    font-family: inherit !important;
    font-size: inherit !important;
    font-weight: inherit !important;
    line-height: inherit !important;
    color: inherit !important;
    text-decoration: none !important;
  }}
  /**
   * Fix centering issues in Android 4.4.
   */
  div[style*=""margin: 16px 0;""] {{
    margin: 0 !important;
  }}
  body {{
    width: 100% !important;
    height: 100% !important;
    padding: 0 !important;
    margin: 0 !important;
  }}
  /**
   * Collapse table borders to avoid space between cells.
   */
  table {{
    border-collapse: collapse !important;
  }}
  a {{
    color: #1a82e2;
  }}
  img {{
    height: auto;
    line-height: 100%;
    text-decoration: none;
    border: 0;
    outline: none;
  }}
  </style>

</head>
<body style=""background-color: #e9ecef;"">

  <div class=""preheader"" style=""display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;"">
    A preheader is the short summary text that follows the subject line when an email is viewed in the inbox.
  </div>
  <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
    <tr>
      <td align=""center"" bgcolor=""#e9ecef"">
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
          <tr>
            <td align=""center"" valign=""top"" style=""padding: 36px 24px;"">
              <a href=""https://www.blogdesire.com"" target=""_blank"" style=""display: inline-block;"">
                <img src=""https://www.blogdesire.com/wp-content/uploads/2019/07/blogdesire-1.png"" alt=""Logo"" border=""0"" width=""48"" style=""display: block; width: 48px; max-width: 48px; min-width: 48px;"">
              </a>
            </td>
          </tr>
        </table>
      </td>
    </tr>
    <tr>
      <td align=""center"" bgcolor=""#e9ecef"">
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
          <tr>
            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;"">
              <h1 style=""text-align: center; margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;"">
                Xác thực email của bạn</h1>
            </td>
      </td>
    </tr>
    <tr>
      <td align=""center"" bgcolor=""#e9ecef"">
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
          <tr>
            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;"">
              <p style=""margin: 0; text-align: center;"">Bạn còn bước cuối cùng để hoàn tất đăng kí tài khoản tại ZLEARN. <br> Vui lòng ấn vào nút dưới đây để xác nhận email</p>
            </td>
          </tr>
          <!-- end copy -->

          <!-- start button -->
          <tr>
            <td align=""left"" bgcolor=""#ffffff"">
              <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                <tr>
                  <td align=""center"" bgcolor=""#ffffff"" style=""padding: 12px;"">
                    <table border=""0"" cellpadding=""0"" cellspacing=""0"">
                      <tr>
                        <td align=""center"" bgcolor=""#1a82e2"" style=""border-radius: 6px;"">
                          <a href=""{confirmUrl}"" target=""_blank"" style=""display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;"">
                            Xác thực
                            </a>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <!-- end button -->

          <!-- start copy -->
          <tr>
            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf"">
              <p style=""margin: 0; text-align: center; font-weight: 500; font-style: oblique;"">ZLEARN - BOT</p>
            </td>
          </tr>
          <!-- end copy -->

        </table>
        <!--[if (gte mso 9)|(IE)]>
        </td>
        </tr>
        </table>
        <![endif]-->
      </td>
    </tr>
    <!-- end copy block -->

    <!-- start footer -->
    <tr>
      <td align=""center"" bgcolor=""#e9ecef"" style=""padding: 24px;"">
        <!--[if (gte mso 9)|(IE)]>
        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
        <tr>
        <td align=""center"" valign=""top"" width=""600"">
        <![endif]-->
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">

          <!-- start permission -->
          <tr>
            <td align=""center"" bgcolor=""#e9ecef"" style=""padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;"">
              <p style=""margin: 0;"">Email được gửi tự động bởi hệ thống. Vui lòng không phản hồi.</p>
            </td>
          </tr>
          <!-- end permission -->

        </table>
        <!--[if (gte mso 9)|(IE)]>
        </td>
        </tr>
        </table>
        <![endif]-->
      </td>
    </tr>
    <!-- end footer -->

  </table>
  <!-- end body -->

</body>
</html>";
        }
        private async Task<Token> GenerateToken(AppUser user)
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
                refreshToken = Convert.ToBase64String(randomBytes);
            }

            return new Token
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

                ValidIssuer = _config[Consts.AppSettingsKey.ISSUER],
                ValidAudience = _config[Consts.AppSettingsKey.AUDIENCE],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Consts.EnvKey.SECRET_KEY)))
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
        #endregion

    }
}
