using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BE.Middlewares
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TestMiddleware> _logger;

        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public void Invoke(HttpContext context) {
            _logger.LogInformation("Test");
            _next(context);
        }
    }
}
