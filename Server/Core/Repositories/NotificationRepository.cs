using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Data;
using Data.Entities.SystemEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class NotificationRepository : BaseRepository<Notification, int>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        public void CreateUserNotification(UserNotification un)
        {
            _context.UserNotifications.Add(un);
        }

        public async Task<List<Notification>> GetNotificationsOfUser(Guid userId)
        {
            var query = from un in _context.UserNotifications
                      join n in _context.Notifications on un.NotificationId equals n.Id
                      where un.UserId == userId
                      select n;
            return await query.ToListAsync();
        }

        public async Task<Guid> GetUserId(int notificationId)
        {
            var query = from un in _context.UserNotifications
                         where un.NotificationId == notificationId
                         select un.UserId;
            return await query.FirstOrDefaultAsync();
        }

        public async Task MarkAsRead(int notificationId)
        {
            var res = await _context.Database
                .ExecuteSqlInterpolatedAsync(@$"
                UPDATE Notifications 
                SET isRead = true 
                WHERE Id = {notificationId}
            ");
            if (res == 0)
            {
                throw new ErrorException("Cập nhật thông báo thất bại");
            }
        }
    }
}
