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
                Id = notification.Id.ToString(),
                Title = notification.Title,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt,
                IsReaded = notification.IsReaded,
                Type = notification.Type
            };
        }

        public static Notification MapFromCreate(CreateNotificationDTO data)
        {
            return new Notification
            {
                Id = Guid.NewGuid(),
                Title = data.Title,
                Message = data.Message,
                CreatedAt = DateTime.Now,
                IsReaded = false,
                Type = data.Type
            };
        }
    }
}
