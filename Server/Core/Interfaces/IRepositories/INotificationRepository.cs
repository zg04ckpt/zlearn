using Core.DTOs;
using Data.Entities.NotificationEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface INotificationRepository : IBaseRepository<Notification, int>
    {
        Task<HashSet<int>> GetReadNotifications(Guid userId);
        Task MaskRead(int notificationId, Guid userId);
        Task<Guid> GetUserId(int notificationId);
        void CreateUserNotification(UserNotification un);
    }
}
