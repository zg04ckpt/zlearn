using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Common
{
    public interface IEmailService
    {
        Task<bool> SendTo(string receiver, string subject, string message);
    }
}
