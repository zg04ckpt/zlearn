using Core.DTOs;
using Data.Entities.SystemEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mappers
{
    public class NotificationMapper
    {
        public static NotificationDTO MapToDTO(Notification notification)
        {
            return new NotificationDTO
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead,
                Type = notification.Type
            };
        }

        public static Notification MapFromCreate(CreateNotificationDTO data)
        {
            return new Notification
            {
                Title = data.Title,
                Message = data.Message,
                CreatedAt = DateTime.Now,
                IsRead = false,
                Type = data.Type
            };
        }
    }
}
