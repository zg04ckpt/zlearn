using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Managements
{
    [Route("api/managements/notifications")]
    [ApiController]
    [Authorize(Consts.ADMIN_ROLE)]
    public class NotificationManagementController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationManagementController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotificatiion(CreateNotificationDTO data)
        {
            return Ok(await _notificationService.CreateNewNotification(data, User));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotificatiions(int pageIndex, int pageSize)
        {
            return Ok(await _notificationService.GetAllSystemNotifications(User, pageIndex, pageSize));
        }

        [HttpGet("{notificationId}/user-id")]
        public async Task<IActionResult> GetTargetUserId(int notificationId)
        {
            return Ok(await _notificationService.GetUserId(User, notificationId));
        }

        [HttpPut("{notificationId}")]
        public async Task<IActionResult> UpdateNotification(int notificationId, UpdateNotificationDTO data)
        {
            return Ok(await _notificationService.Update(User, notificationId, data));
        }

        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            return Ok(await _notificationService.Delete(User, notificationId));
        }
    }
}
