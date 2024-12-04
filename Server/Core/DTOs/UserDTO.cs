
using Data.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{

    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> RoleNames { get; set; }
    }

    public class UserInfoDTO
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? ImageUrl { get; set; }
        public string? DayOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
        public bool IsFriend { get; set; }
        public string? PhoneNum { get; set; }
        public string? Address { get; set; }
        public string? Intro { get; set; }
        public string? SocialLinks { get; set; }
    }

    public class UserProfileDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? DayOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string? PhoneNum { get; set; }
        public string? Address { get; set; }
        public string? Intro { get; set; }
        public string? SocialLinks { get; set; }
    }

    public class UserManagementDTO
    {
        public string? ImageUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public string CreatedDate { get; set; }
        public string? Description { get; set; }
        public string? UserLinks { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class UserManagementSearchDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? CreatedDate { get; set; }
    }
}
