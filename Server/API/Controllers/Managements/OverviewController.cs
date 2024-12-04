using API.Authorization;
using Core.DTOs;
using Core.Interfaces.IServices.System;
using Core.RealTime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Controllers.Managements
{
    [Route("api/managements/overview")]
    [ApiController]
    [Authorize("Admin")]
    public class OverviewController : ControllerBase
    {
        private readonly ISummaryService _summaryService;
        private readonly IHubContext<LogHub> _logHubContext;

        public OverviewController(ISummaryService summaryService, IHubContext<LogHub> logHubContext)
        {
            _summaryService = summaryService;
            _logHubContext = logHubContext;
        }

        [HttpGet("today")]
        public IActionResult GetToday()
        {
            return Ok(_summaryService.GetToday());
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetRange(string start, string end)
        {
            return Ok(await _summaryService.GetByRange(start, end));
        }

        [HttpPost("connectToLogHub")]
        public async Task<IActionResult> ConnectToLogHub([FromBody] ConnectionDTO data)
        {
            await _logHubContext.Groups.AddToGroupAsync(data.ConnectionId, "log");
            return Ok();
        }
    }
}
