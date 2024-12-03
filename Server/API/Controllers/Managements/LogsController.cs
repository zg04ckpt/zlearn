using API.Authorization;
using Core.Interfaces.IServices.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Managements
{
    [Route("api/managements/log")]
    [ApiController]
    [Authorize("Admin")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogsOfDate([FromQuery]string date)
        {
            return Ok(await _logService.GetLogsOfDate(date));
        }
    }
}
