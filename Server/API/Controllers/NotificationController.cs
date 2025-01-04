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
        public async Task<IActionResult> GetNotification(int start, int max)
        {
            return Ok(await _notificationService.GetNotifications(User, start, max));
        }

        [HttpPost]
        public async Task<IActionResult> Connect(ConnectionDTO connection)
        {
            return Ok(await _notificationService.SignalRConnect(User, connection.ConnectionId));
        }
    }
}
