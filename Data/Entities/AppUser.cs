
using Data.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string CreatedDate { get; set; }
        public string RefreshToken { get; set; }
        public string ImageUrl { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        #region full information
        public string Description { get; set; }
        public string UserLinks { get; set; }
        #endregion

        public List<Test> Tests { get; set; }
        public List<SavedTest> UserInTests { get; set; }
    }
}
