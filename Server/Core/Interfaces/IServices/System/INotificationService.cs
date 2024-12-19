using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.System
{
    public interface INotificationService
    {
        Task<APIResult> CreateNewNotification(CreateNotificationDTO data, ClaimsPrincipal claims);
        Task<APIResult<List<NotificationDTO>>> GetNotifications(ClaimsPrincipal claims);
    }
}
