using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RealTime
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;

        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine(Context.UserIdentifier);
            return base.OnConnectedAsync();
        }

        public async Task OnReadNotification(int notificationId)
        {
            if (!string.IsNullOrEmpty(Context.UserIdentifier))
            {
                await _notificationService.ReadNotification(notificationId, Guid.Parse(Context.UserIdentifier));
            }
        }
    }
}
