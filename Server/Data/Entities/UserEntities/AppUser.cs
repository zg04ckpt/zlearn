using Data.Entities.CommonEntities;
using Data.Entities.DocumentEntities;
using Data.Entities.Enums;
using Data.Entities.NotificationEntities;
using Data.Entities.PostEnttities;
using Data.Entities.SystemEntities;
using Data.Entities.TestEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.UserEntities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string RefreshToken { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public int Likes { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        #region full information
        public string Description { get; set; }
        public string UserLinks { get; set; }
        #endregion

        //Rela
        public List<Test> Tests { get; set; }
        public List<Post> Posts { get; set; }
        public List<Document> Documents { get; set; }
        public List<SavedTest> UserInTests { get; set; }
        public List<Comment> Comments { get; set; }
        public List<UserNotification> UserNotifications { get; set; }
        public List<ReadNotification> ReadNotifications { get; set; }
        public List<Order> Orders { get; set; }
    }
}
