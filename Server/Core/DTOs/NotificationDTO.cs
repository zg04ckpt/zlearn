using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class NotificationDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsReaded { get; set; }
        public NotificationType Type { get; set; }
    }

    public class CreateNotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
    }
}
