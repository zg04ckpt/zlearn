using Core.DTOs;
using Data.Entities;

namespace Core.Mappers
{
    public class UserMapper
    {
        protected UserManagementDTO Map(AppUser user)
        {
            return new UserManagementDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                CreatedDate = user.CreatedDate,
                Description = user.Description,
                UserLinks = user.UserLinks,
                Id = user.Id.ToString(),
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnd = user.LockoutEnd,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount
            };
        }

        protected AppUser Map(AppUser oldUser, UserManagementDTO user)
        {
            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Address = user.Address;
            oldUser.Gender = user.Gender;
            oldUser.DateOfBirth = user.DateOfBirth;
            oldUser.CreatedDate = user.CreatedDate;
            oldUser.Description = user.Description;
            oldUser.UserLinks = user.UserLinks;
            oldUser.UserName = user.UserName;
            oldUser.Email = user.Email;
            oldUser.EmailConfirmed = user.EmailConfirmed;
            oldUser.PhoneNumber = user.PhoneNumber;
            oldUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            oldUser.TwoFactorEnabled = user.TwoFactorEnabled;
            oldUser.LockoutEnd = user.LockoutEnd;
            oldUser.LockoutEnabled = user.LockoutEnabled;
            oldUser.AccessFailedCount = user.AccessFailedCount;

            return oldUser;
        }
    }
}
