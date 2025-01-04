using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.System;
using Core.Mappers;
using Core.RealTime;
using Data.Entities.Enums;
using Data.Entities.NotificationEntities;
using Data.Entities.SystemEntities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }

        public async Task<APIResult> CreateNewNotification(CreateNotificationDTO data, ClaimsPrincipal claims)
        {
            if(!claims.IsInRole("Admin"))
            {
                throw new ForbiddenException();
            }

            var notification = NotificationMapper.MapFromCreate(data);
            _notificationRepository.Create(notification);
            await _notificationRepository.SaveChanges();

            // If this is notification of user
            if (data.Type == NotificationType.User)
            {
                if(string.IsNullOrEmpty(data.UserId))
                {
                    throw new ErrorException("ID người dùng trống!");
                }
                _notificationRepository.CreateUserNotification(new UserNotification
                {
                    UserId = Guid.Parse(data.UserId),
                    NotificationId = notification.Id
                });
                await _notificationRepository.SaveChanges();
            }

            //Send notificaion to client
            await SendToHub(NotificationMapper.MapToDTO(notification), data.UserId);

            return new APISuccessResult();
        }
        
        public async Task<APIResult<PaginatedResult<NotificationDTO>>> GetAllSystemNotifications(ClaimsPrincipal claims, int pageIndex, int pageSize)
        {
            if (!claims.IsInRole("Admin"))
            {
                throw new ForbiddenException();
            }

            var query = _notificationRepository.GetQuery().AsNoTracking();
            var total = await query.CountAsync();
            var data = query
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pageIndex-1) * pageSize)
                .Take(pageSize)
                .Select(e => NotificationMapper.MapToDTO(e));

            return new APISuccessResult<PaginatedResult<NotificationDTO>>(new PaginatedResult<NotificationDTO>
            {
                Data = data,
                Total = total
            });
        }

        public async Task<APIResult<List<NotificationDTO>>> GetNotifications(ClaimsPrincipal claims, int start, int max)
        {
            //Skip and only take max noti
            var query = _notificationRepository.GetQuery().AsNoTracking();

            //Find for anonymous
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                query = query.Where(e => e.Type == NotificationType.System);
            }

            query = query
                .OrderByDescending(e => e.CreatedAt)
                .Skip(start)
                .Take(max);

            // Get system notification
            var notifications = await query
                .Select(e => NotificationMapper.MapToDTO(e))
                .ToListAsync();

            // Check is read
            if(!string.IsNullOrEmpty(userId))
            {
                var set = await _notificationRepository.GetReadNotifications(Guid.Parse(userId));
                notifications.ForEach(e =>
                {
                    if(set.Contains(e.Id))
                    {
                        e.IsRead = true;
                    }    
                    else
                    {
                        e.IsRead = false;
                    }    
                });
            }    

            return new APISuccessResult<List<NotificationDTO>>(notifications);
        }

        public async Task<APIResult> Delete(ClaimsPrincipal claims, int notificationId)
        {
            if (!claims.IsInRole("Admin"))
            {
                throw new ForbiddenException();
            }
            
            var notification = await _notificationRepository.GetById(notificationId)
                ?? throw new ErrorException("Thông báo không tồn tại!");

            _notificationRepository.Delete(notification);
            await _notificationRepository.SaveChanges();

            return new APISuccessResult<string>(notification.Id.ToString());
        }

        public async Task<APIResult<string>> GetUserId(ClaimsPrincipal claims, int notificationId)
        {
            if (!claims.IsInRole("Admin"))
            {
                throw new ForbiddenException();
            }

            var userId = await _notificationRepository.GetUserId(notificationId);
            if(userId == default)
            {
                throw new ErrorException("Không tìm thấy người dùng");
            }

            return new APISuccessResult<string>(userId.ToString());
        }

        public async Task<APIResult> Update(ClaimsPrincipal claims, int notificationId, UpdateNotificationDTO data)
        {
            if (!claims.IsInRole("Admin"))
            {
                throw new ForbiddenException();
            }

            var notification = await _notificationRepository.GetById(notificationId)
                ?? throw new ErrorException("Thông báo không tồn tại!");

            notification.Title = data.Title;
            notification.Message = data.Message;

            _notificationRepository.Update(notification);
            await _notificationRepository.SaveChanges();

            return new APISuccessResult("Cập nhật thành công!");
        }

        public async Task SendToHub(NotificationDTO notification, string? userId)
        {
            if(notification.Type == NotificationType.System)
            {
                await _hubContext.Clients.All.SendAsync("onHasNewNotification", notification);
            }
            else if(notification.Type == NotificationType.User)
            {
                await _hubContext.Clients.Group(userId).SendAsync("onHasNewNotification", notification);
            }    
        }

        public async Task<APIResult> SignalRConnect(ClaimsPrincipal claims, string connectionId)
        {
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _hubContext.Groups.AddToGroupAsync(connectionId, userId);
            return new APISuccessResult();
        }

        public async Task ReadNotification(int notificationId, Guid userId)
        {
            await _notificationRepository.MaskRead(notificationId, userId);
        }

        public async Task CreateNewNotification(CreateNotificationDTO data)
        {
            var notification = NotificationMapper.MapFromCreate(data);
            _notificationRepository.Create(notification);
            await _notificationRepository.SaveChanges();

            // If this is notification of user
            if (data.Type == NotificationType.User)
            {
                if (string.IsNullOrEmpty(data.UserId))
                {
                    throw new ErrorException("ID người dùng trống!");
                }
                _notificationRepository.CreateUserNotification(new UserNotification
                {
                    UserId = Guid.Parse(data.UserId),
                    NotificationId = notification.Id
                });
                await _notificationRepository.SaveChanges();
            }

            //Send notificaion to client
            await SendToHub(NotificationMapper.MapToDTO(notification), data.UserId);
        }
    }
}
