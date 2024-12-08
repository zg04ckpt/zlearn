using Data.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.SystemEntities
{
    public class UserNotification
    {
        public Guid UserId { get; set; }
        public Guid NotificationId { get; set; }

        //Rela
        public AppUser User { get; set; }
        public Notification Notification { get; set; }
    }
}
