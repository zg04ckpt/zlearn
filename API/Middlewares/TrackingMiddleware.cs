using Core.Interfaces.IServices.System;
using System.Collections.Concurrent;

namespace API.Middlewares
{
    public class TrackingMiddleware : IMiddleware
    {
        private readonly ISummaryService _summaryService;

        public TrackingMiddleware(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            if (ipAddress != null)
            {
                await _summaryService.IncreaseAccessCount(ipAddress);
            }

            await next(context);
        }
    }
}
