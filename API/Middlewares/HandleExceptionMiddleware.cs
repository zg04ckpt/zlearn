using Core.Common;
using Core.Exceptions;
using Newtonsoft.Json;
using System.Text.Json;

namespace API.Middlewares
{
    public class HandleExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public HandleExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            if (ex is ForbiddenException)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                var result = JsonConvert.SerializeObject(new APIErrorResult(ex.Message));
                await context.Response.WriteAsync(result);
            }
            else if (ex is ErrorException)
            {
                var errorEx = ex as ErrorException;
                var result = JsonConvert.SerializeObject(new APIErrorResult(errorEx.Message, errorEx.Errors));
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(result);
            }
            else
            {
                context.Response.StatusCode= StatusCodes.Status500InternalServerError;
                var result = JsonConvert.SerializeObject(new APIErrorResult("Lỗi không xác định"));
                await context.Response.WriteAsync(result);
            }
            
        }
    }
}
