using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.NotificationEntities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType Type { get; set; }

        //Rela
        public List<UserNotification> UserNotifications { get; set; }
        public List<ReadNotification> ReadNotifications { get; set; }
    }
}
