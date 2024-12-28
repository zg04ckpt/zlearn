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
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public NotificationType Type { get; set; }
    }

    public class CreateNotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public string? UserId { get; set; }
    }

    public class UpdateNotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
