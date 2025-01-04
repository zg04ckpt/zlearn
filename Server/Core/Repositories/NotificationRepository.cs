using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Data;
using Data.Entities.NotificationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        public async Task<HashSet<int>> GetReadNotifications(Guid userId)
        {
            HashSet<int> set = new();
            var readNotifications = await _context.ReadNotifications
                .Where(x => x.UserId == userId)
                .ToListAsync();
            readNotifications.ForEach(e => set.Add(e.NotificationId));
            return set;
        }

        public async Task<Guid> GetUserId(int notificationId)
        {
            var query = from un in _context.UserNotifications
                         where un.NotificationId == notificationId
                         select un.UserId;
            return await query.FirstOrDefaultAsync();
        }

        public async Task MaskRead(int notificationId, Guid userId)
        {
            _context.Add(new ReadNotification
            {
                NotificationId = notificationId,
                UserId = userId
            });
            await _context.SaveChangesAsync();
        }
    }
}
