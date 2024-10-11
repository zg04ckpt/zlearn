using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.System.Roles;

namespace ViewModels.System.Manage
{
    public class UserManagementModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Address { get; set; } 
        public Gender Gender { get; set; } 
        public string DateOfBirth { get; set; } 
        public string CreatedDate { get; set; } 
        public string RefreshToken { get; set; } 
        public DateTime RefreshTokenExpiryTime { get; set; } 
        public string Description { get; set; } 
        public string UserLinks { get; set; } 
        public Guid Id { get; set; } 
        public string UserName { get; set; } 
        public string Email { get; set; } 
        public bool EmailConfirmed { get; set; } 
        public string PhoneNumber { get; set; } 
        public bool PhoneNumberConfirmed { get; set; } 
        public bool TwoFactorEnabled { get; set; } 
        public DateTimeOffset? LockoutEnd { get; set; } 
        public bool LockoutEnabled { get; set; } 
        public int AccessFailedCount { get; set; } 
        public List<string> Roles { get; set; }
    }
}
