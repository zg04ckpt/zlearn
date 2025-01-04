using Core.DTOs;
using Data.Entities.NotificationEntities;
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
                Type = data.Type
            };
        }
    }
}
