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
        Task<APIResult<List<NotificationDTO>>> GetNotifications(ClaimsPrincipal claims, int start, int max);
        Task<APIResult<PaginatedResult<NotificationDTO>>> GetAllSystemNotifications(ClaimsPrincipal claims, int pageIndex, int pageSize);
        Task<APIResult> Delete(ClaimsPrincipal claims, int notificationId);
        Task<APIResult<string>> GetUserId(ClaimsPrincipal claims, int notificationId);
        Task<APIResult> Update(ClaimsPrincipal claims, int notificationId, UpdateNotificationDTO data);
        Task SendToHub(NotificationDTO notification, string? userId);
        Task<APIResult> SignalRConnect(ClaimsPrincipal claims, string connectionId);
        Task ReadNotification(int notificationId, Guid userId);
        Task CreateNewNotification(CreateNotificationDTO data);
    }
}
