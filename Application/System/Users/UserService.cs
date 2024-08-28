using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Application.Common;
using ViewModels.System.Users;
using Data;
using Utilities.Exceptions;

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
        
    }
}
