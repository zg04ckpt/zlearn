using Data.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.NotificationEntities
{
    public class ReadNotification
    {
        public int NotificationId { get; set; }
        public Guid UserId { get; set; }

        //Rela
        public Notification Notification { get; set; }
        public AppUser User { get; set; }
    }
}
