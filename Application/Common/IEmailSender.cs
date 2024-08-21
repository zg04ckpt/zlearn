﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IEmailSender
    {
        Task<bool> SendTo(string receiver, string subject, string message);
    }
}
