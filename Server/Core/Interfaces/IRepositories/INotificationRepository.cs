﻿using Data.Entities.SystemEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface INotificationRepository : IBaseRepository<Notification, int>
    {
        Task<List<Notification>> GetNotificationsOfUser(Guid userId);
        Task<Guid> GetUserId(int notificationId);
        void CreateUserNotification(UserNotification un);
        Task MarkAsRead(int notificationId);
    }
}
