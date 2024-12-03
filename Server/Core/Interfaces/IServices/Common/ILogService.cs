using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Common
{
    public interface ILogService
    {
        Task SendInfoLog(string content);
        Task SendErrorLog(string content);
        Task<APIResult<List<LogDTO>>> GetLogsOfDate(string date);
    }
}
