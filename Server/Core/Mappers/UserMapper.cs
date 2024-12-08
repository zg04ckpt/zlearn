using Core.DTOs;
using Data.Entities.UserEntities;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Core.Mappers
{
    public class UserMapper
    {
        public static UserManagementDTO MapToManage(AppUser user)
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

        public static AppUser MapFromManage(AppUser oldUser, UserManagementDTO user)
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

        public static UserProfileDTO MapToProfile(AppUser user)
        {
            return new UserProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNum = user.PhoneNumber,
                Gender = user.Gender,
                DayOfBirth = user.DateOfBirth,
                Address = user.Address,
                Intro = user.Description,
                SocialLinks = user.UserLinks,
                ImageUrl = user.ImageUrl
            };
        }

        public static UserInfoDTO MapToInfo(AppUser user)
        {
            return new UserInfoDTO
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                PhoneNum = user.PhoneNumber,
                Gender = user.Gender,
                DayOfBirth = user.DateOfBirth,
                Address = user.Address,
                Intro = user.Description,
                SocialLinks = user.UserLinks,
                ImageUrl = user.ImageUrl,
                Likes = user.Likes
            };
        }

        public static AppUser MapFromProfile(AppUser oldUser, UserProfileDTO user)
        {
            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Address = user.Address;
            oldUser.Gender = user.Gender;
            oldUser.DateOfBirth = user.DayOfBirth;
            oldUser.Email = user.Email;
            oldUser.PhoneNumber = user.PhoneNum;
            oldUser.Description = user.Intro;
            oldUser.UserLinks = user.SocialLinks;

            return oldUser;
        }
    }
}
