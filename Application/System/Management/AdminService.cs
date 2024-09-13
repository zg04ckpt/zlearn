using Application.Common;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using ViewModels.Common;
using ViewModels.Features.Learn.Test.Question;
using ViewModels.System.Manage;

namespace Application.System.Manage
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileService _fileService;

        public AdminService(AppDbContext context, UserManager<AppUser> userManager, IFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            _fileService = fileService;
        }

        #region manage users

        public async Task AssignRole(string userId, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User không tồn tại");

            var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
            if (user.Email == adminEmail)
                throw new BadRequestException("Không thể sửa quyền của Admin");

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

        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User không tồn tại");
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<string>> GetAllRolesOfUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User không tồn tại"); ;

            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public async Task<PagingResponse<UserManagementModel>> GetAllUsers(PagingRequest request)
        {
            var users = await _userManager.Users
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            var userModels = new List<UserManagementModel>();
            foreach (var user in users)
            {
                userModels.Add(new UserManagementModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    CreatedDate = user.CreatedDate,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                    Description = user.Description,
                    UserLinks = user.UserLinks,
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    LockoutEnd = user.LockoutEnd,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    Roles = (List<string>)await _userManager.GetRolesAsync(user)
                });
            }

            return new PagingResponse<UserManagementModel>
            {
                Total = await _userManager.Users.CountAsync(),
                Data = userModels
            };
        }

        public async Task<UserManagementModel> GetUserById(string id)
        {
            var ap = await _userManager.FindByIdAsync(id);
            if (ap == null)
                throw new NotFoundException("User không tồn tại");
            return new UserManagementModel
            {
                FirstName = ap.FirstName,
                LastName = ap.LastName,
                Address = ap.Address,
                Gender = ap.Gender,
                DateOfBirth = ap.DateOfBirth,
                CreatedDate = ap.CreatedDate,
                RefreshToken = ap.RefreshToken,
                RefreshTokenExpiryTime = ap.RefreshTokenExpiryTime,
                Description = ap.Description,
                UserLinks = ap.UserLinks,
                Id = ap.Id,
                UserName = ap.UserName,
                Email = ap.Email,
                EmailConfirmed = ap.EmailConfirmed,
                PhoneNumber = ap.PhoneNumber,
                PhoneNumberConfirmed = ap.PhoneNumberConfirmed,
                TwoFactorEnabled = ap.TwoFactorEnabled,
                LockoutEnd = ap.LockoutEnd,
                LockoutEnabled = ap.LockoutEnabled,
                AccessFailedCount = ap.AccessFailedCount
            };
        }

        public async Task UpdateUser(UserManagementModel user)
        {
            var oldUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (oldUser == null)
                throw new NotFoundException("Không tìm thấy user");

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Address = user.Address;
            oldUser.Gender = user.Gender;
            oldUser.DateOfBirth = user.DateOfBirth;
            oldUser.CreatedDate = user.CreatedDate;
            oldUser.RefreshToken = user.RefreshToken;
            oldUser.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
            oldUser.Description = user.Description;
            oldUser.UserLinks = user.UserLinks;
            oldUser.UserName = user.UserName;
            oldUser.Email = user.Email;
            oldUser.EmailConfirmed = user.EmailConfirmed;
            oldUser.PhoneNumber = user.PhoneNumber;
            oldUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            oldUser.TwoFactorEnabled = user.TwoFactorEnabled;
            oldUser.LockoutEnd = oldUser.LockoutEnd;
            oldUser.LockoutEnabled = oldUser.LockoutEnabled;
            oldUser.AccessFailedCount = user.AccessFailedCount;

            await _userManager.UpdateAsync(oldUser);
        }

        public async Task<PagingResponse<UserManagementModel>> FindByEmail(string key, PagingRequest request)
        {
            var users = await _userManager.Users
                .Where(u => u.Email.Contains(key))
                .ToListAsync();

            var total = users.Count;

            users = users
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize).ToList();

            var userModels = new List<UserManagementModel>();
            foreach (var user in users)
            {
                userModels.Add(new UserManagementModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    CreatedDate = user.CreatedDate,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                    Description = user.Description,
                    UserLinks = user.UserLinks,
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    LockoutEnd = user.LockoutEnd,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    Roles = (List<string>)await _userManager.GetRolesAsync(user)
                });
            }

            return new PagingResponse<UserManagementModel>
            {
                Total = total,
                Data = userModels
            };
        }

        public async Task<PagingResponse<UserManagementModel>> FindByName(string key, PagingRequest request)
        {
            var users = await _userManager.Users
                .Where(u => (u.LastName + " " + u.FirstName).Contains(key))
                .ToListAsync();

            var total = users.Count;

            users = users
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize).ToList();

            var userModels = new List<UserManagementModel>();
            foreach (var user in users)
            {
                userModels.Add(new UserManagementModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    CreatedDate = user.CreatedDate,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                    Description = user.Description,
                    UserLinks = user.UserLinks,
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    LockoutEnd = user.LockoutEnd,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    Roles = (List<string>)await _userManager.GetRolesAsync(user)
                });
            }

            return new PagingResponse<UserManagementModel>
            {
                Total = total,
                Data = userModels
            };
        }

        public async Task<PagingResponse<UserManagementModel>> FindByPhoneNum(string key, PagingRequest request)
        {
            var users = await _userManager.Users
                .Where(u => u.PhoneNumber.Contains(key))
                .ToListAsync();

            var total = users.Count;

            users = users
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            var userModels = new List<UserManagementModel>();
            foreach (var user in users)
            {
                userModels.Add(new UserManagementModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    CreatedDate = user.CreatedDate,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                    Description = user.Description,
                    UserLinks = user.UserLinks,
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    LockoutEnd = user.LockoutEnd,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    Roles = (List<string>)await _userManager.GetRolesAsync(user)
                });
            }

            return new PagingResponse<UserManagementModel>
            {
                Total = total,
                Data = userModels
            };
        }

        public async Task<PagingResponse<UserManagementModel>> FindByRole(string key, PagingRequest request)
        {
            var all = await _userManager.Users.ToListAsync();
            var users = new List<UserManagementModel>();

            foreach (var user in all)
            {
                if (await _userManager.IsInRoleAsync(user, key))
                {
                    users.Add(new UserManagementModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = user.Address,
                        Gender = user.Gender,
                        DateOfBirth = user.DateOfBirth,
                        CreatedDate = user.CreatedDate,
                        RefreshToken = user.RefreshToken,
                        RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                        Description = user.Description,
                        UserLinks = user.UserLinks,
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        PhoneNumber = user.PhoneNumber,
                        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        LockoutEnd = user.LockoutEnd,
                        LockoutEnabled = user.LockoutEnabled,
                        AccessFailedCount = user.AccessFailedCount,
                        Roles = (List<string>)await _userManager.GetRolesAsync(user)
                    });
                }
            }

            var total = users.Count;

            users = users
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            return new PagingResponse<UserManagementModel>
            {
                Total = total,
                Data = users
            };
        }

        public async Task<PagingResponse<UserManagementModel>> FindByUserName(string key, PagingRequest request)
        {
            var users = await _userManager.Users
            .Where(u => u.UserName.Contains(key)).ToListAsync();

            var total = users.Count;

            users = users
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize).ToList();

            var userModels = new List<UserManagementModel>();
            foreach (var user in users)
            {
                userModels.Add(new UserManagementModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    CreatedDate = user.CreatedDate,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                    Description = user.Description,
                    UserLinks = user.UserLinks,
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    LockoutEnd = user.LockoutEnd,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    Roles = (List<string>)await _userManager.GetRolesAsync(user)
                });
            }


            return new PagingResponse<UserManagementModel>
            {
                Total = total,
                Data = userModels
            };
        }


        #endregion

        #region manage tests
        public async Task<PagingResponse<TestManagementModel>> GetTests(PagingRequest request)
        {
            var tests = await _context.Tests.ToListAsync();
            return new PagingResponse<TestManagementModel>
            {
                Total = tests.Count,
                Data = tests
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Take(request.PageSize)
                .Select(x => new TestManagementModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    ImageUrl = _fileService.GetFileUrl(x.ImageUrl),
                    UpdatedDate = x.UpdatedDate,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    Source = x.Source,
                    Duration = x.Duration,
                    AuthorName = x.AuthorName,
                    NumberOfAttempts = x.NumberOfAttempts,
                    NumberOfQuestions = x.NumberOfQuestions,
                    IsPrivate = x.IsPrivate,
                    AuthorId = x.AuthorId.ToString(),
                }).ToList()
            };
        }

        public async Task<PagingResponse<TestManagementModel>> GetTestsByUserId(string userId, PagingRequest request)
        {
            var tests = await _context.Tests
                .Where(x => x.AuthorId.ToString() == userId)
                .ToListAsync();
            return new PagingResponse<TestManagementModel>
            {
                Total = tests.Count,
                Data = tests
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Take(request.PageSize)
                .Select(x => new TestManagementModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    ImageUrl = _fileService.GetFileUrl(x.ImageUrl),
                    UpdatedDate = x.UpdatedDate,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    Source = x.Source,
                    Duration = x.Duration,
                    AuthorName = x.AuthorName,
                    NumberOfAttempts = x.NumberOfAttempts,
                    NumberOfQuestions = x.NumberOfQuestions,
                    IsPrivate = x.IsPrivate,
                    AuthorId = x.AuthorId.ToString(),
                }).ToList()
            };
        }

        public async Task<List<QuestionUpdateContent>> GetQuestions(string testId)
        {
            return await _context.Questions
                .Where(x => x.TestId.ToString() == testId)
                .Select(x => new QuestionUpdateContent
                {
                    Id = x.Id.ToString(),
                    Content = x.Content,
                    AnswerA = x.AnswerA,
                    AnswerB = x.AnswerB,
                    AnswerC = x.AnswerC,
                    AnswerD = x.AnswerD,
                    CorrectAnswer = x.CorrectAnswer
                })
                .ToListAsync();
        }

        public async Task UpdateTest(TestManagementModel request)
        {
            var oldTest = await _context.Tests.FindAsync(Guid.Parse(request.Id))
                ?? throw new BadRequestException("Không tìm thấy test");
            oldTest.Name = request.Name;
            oldTest.Description = request.Description;

        }

        public Task UpdateQuestions(string testId, QuestionUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTest(string testId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
