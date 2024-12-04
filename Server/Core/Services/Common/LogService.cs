using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IServices.Common;
using Core.RealTime;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Services.Common
{
    public class LogService : ILogService
    {
        private const string TIME_PATTERN = "dd-MM-yyyy HH:mm:ss";
        private readonly string _logFolderPath;

        private readonly IHubContext<LogHub> _logHubContext;

        public LogService(IHubContext<LogHub> logHubContext)
        {
            _logHubContext = logHubContext;
            _logFolderPath = Path.Combine(AppContext.BaseDirectory, "Logs");
        }

        public async Task<APIResult<List<LogDTO>>> GetLogsOfDate(string date)
        {
            var filename = $"app-{date}.log";
            var filePath = Path.Combine(_logFolderPath, filename);

            if (!File.Exists(filePath))
            {
                throw new ErrorException($"Log file '{filename}' does not exist.");
            }

            var lines = await File.ReadAllLinesAsync(filePath);
            var logs = new List<LogDTO>();
            LogDTO? currentLog = null;

            // Regex to match
            var mainLogPattern = new Regex(
                @"^(?<Time>\d{4}-\d{2}-\d{2} [\d:\.]+ [+-]\d{2}:\d{2}) \[(?<Type>[A-Z]+)\] (?<Desc>.+)$",
                RegexOptions.Compiled);

            foreach (var line in lines)
            {
                var match = mainLogPattern.Match(line);

                if (match.Success)
                {
                    // if has prev log => add to list
                    if (currentLog != null)
                    {
                        logs.Add(currentLog);
                    }

                    // and create new log
                    currentLog = new LogDTO
                    {
                        Time = match.Groups["Time"].Value,
                        Type = match.Groups["Type"].Value,
                        Desc = match.Groups["Desc"].Value,
                        Detail = "" // Chưa có chi tiết
                    };
                }
                else
                {
                    // if it not math => this is a detail of prev log
                    if (currentLog != null)
                    {
                        currentLog.Detail +=  line + "\n";
                    }
                }
            }

            // add last log
            if (currentLog != null)
            {
                logs.Add(currentLog);
            }

            return new APISuccessResult<List<LogDTO>>(logs);
        }


        public async Task SendErrorLog(string content)
        {
            await _logHubContext.Clients.Group("log").SendAsync("onHasLog", new LogDTO
            {
                Time = DateTime.Now.ToString(TIME_PATTERN),
                Type = "[ERR]",
                Desc = content,
                Detail = null
            });
        }

        public async Task SendInfoLog(string content)
        {
            await _logHubContext.Clients.Group("log").SendAsync("onHasLog", new LogDTO
            {
                Time = DateTime.Now.ToString(TIME_PATTERN),
                Type = "[INF]",
                Desc = content,
                Detail = null
            });
        }
    }
}
