using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotification()
        {
            return Ok(await _notificationService.GetNotifications(User));
        }

        [HttpPost]
        [Authorize(Consts.ADMIN_ROLE)]
        public async Task<IActionResult> CreateNotificatiion(CreateNotificationDTO data)
        {
            return Ok(await _notificationService.CreateNewNotification(data, User));
        }
    }
}
