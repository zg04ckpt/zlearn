using Data.Entities.SystemEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface INotificationRepository : IBaseRepository<Notification, Guid>
    {
        Task<List<Notification>> GetNotificationsOfUser(Guid userId);
    }
}
