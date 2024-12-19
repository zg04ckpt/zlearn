using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.System;
using Core.Mappers;
using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.System
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<APIResult> CreateNewNotification(CreateNotificationDTO data, ClaimsPrincipal claims)
        {
            if(!claims.IsInRole("Admin"))
            {
                throw new ErrorException("Chỉ admin mới được phép tạo thông báo!");
            }

            _notificationRepository.Create(NotificationMapper.MapFromCreate(data));
            await _notificationRepository.SaveChanges();

            return new APISuccessResult();
        }

        public async Task<APIResult<List<NotificationDTO>>> GetNotifications(ClaimsPrincipal claims)
        {
            // Get system notification
            var notifications = await _notificationRepository.GetAll(e => e.Type == NotificationType.System);

            // Get user notification
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!string.IsNullOrEmpty(userId))
            {
                notifications = notifications.Concat(await _notificationRepository.GetNotificationsOfUser(Guid.Parse(userId)));
            }    

            //Sort by date
            notifications = notifications.OrderByDescending(e => e.CreatedAt);

            return new APISuccessResult<List<NotificationDTO>>(notifications.Select(e => NotificationMapper.MapToDTO(e)).ToList());
        }
    }
}
