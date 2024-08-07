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
using Utilities;
using Application.Common;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ViewModels.System.Users;
using Org.BouncyCastle.Asn1.Ocsp;
using Data;
using Utilities.Exceptions;
using ViewModels.System.Auth;

namespace Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, IEmailSender emailSender, RoleManager<AppRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<AppUser>> GetUsers(PagingRequest request)
        {
            return await _userManager.Users
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
        }
        public async Task<AppUser> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User không tồn tại");
            return user;
        }
        public async Task<UserDetailModel> GetUserDetail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User không tồn tại");

            return new UserDetailModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNum = user.PhoneNumber,
                Gender = user.Gender,
                DayOfBirth = user.DateOfBirth,
                Address = user.Address,
                Intro = user.Description,
                SocialLinks = user.UserLinks
            };
        }
        public async Task UpdateUserDetail(string id, UserDetailModel request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User không tồn tại");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Address = request.Address;
            user.Gender = request.Gender;
            user.DateOfBirth = request.DayOfBirth;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNum;
            user.Description = request.Intro;
            user.UserLinks = request.SocialLinks;

            await _userManager.UpdateAsync(user);
        }
        public async Task<List<string>> GetAllRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User không tồn tại"); ;

            return (await _userManager.GetRolesAsync(user)).ToList();
        }
        public async Task RoleAssign(string userId, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User không tồn tại");

            var userRoles = new HashSet<string>(await _userManager.GetRolesAsync(user));
            foreach (var role in request.Roles)
            {
                if (!role.Selected)
                {
                    if (userRoles.Contains(role.Name))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
                else
                {
                    if (!userRoles.Contains(role.Name))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
            }
        }

        #region private
        private async Task<Token> GenerateToken(AppUser user)
        {
            //generate access token
            var roles = await _userManager.GetRolesAsync(user);
            var publicClaims = new Claim[]
            {
                new (ClaimTypes.Name, user.UserName), //require for refresh
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
