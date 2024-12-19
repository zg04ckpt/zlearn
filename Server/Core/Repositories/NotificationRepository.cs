using Core.Interfaces.IRepositories;
using Data;
using Data.Entities.SystemEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class NotificationRepository : BaseRepository<Notification, Guid>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Notification>> GetNotificationsOfUser(Guid userId)
        {
            var res = from un in _context.UserNotifications
                      join n in _context.Notifications on un.NotificationId equals n.Id
                      where un.UserId == userId
                      select n;
            return await res.ToListAsync();
        }
    }
}
