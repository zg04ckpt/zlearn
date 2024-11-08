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

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, IEmailService emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailSender = emailSender;
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
                CreatedDate = DateOnly.FromDateTime(DateTime.Today).ToString()
            };

            //save user to db
            var createProcess = await _userManager.CreateAsync(newUser, request.Password);
            if (createProcess.Succeeded)
            {
                var roleAssignResult = await _userManager.AddToRoleAsync(newUser, Consts.USER_ROLE);
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
                        throw new ErrorException("Lỗi gửi mail xác thực");
                    }

                    return new APISuccessResult("Đăng kí tài khoản thành công");
                }
                else
                {
                    await _userManager.DeleteAsync(newUser);
                    throw new ErrorException("Lỗi gán quyền user");
                }
            }
            else
            {
                await _userManager.DeleteAsync(newUser);
                throw new ErrorException("Đăng kí không thành công!");
            }
        }

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
    }
}
