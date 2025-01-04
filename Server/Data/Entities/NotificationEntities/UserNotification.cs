﻿using Data.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.NotificationEntities
{
    public class UserNotification
    {
        public Guid UserId { get; set; }
        public int NotificationId { get; set; }

        //Rela
        public AppUser User { get; set; }
        public Notification Notification { get; set; }
    }
}
