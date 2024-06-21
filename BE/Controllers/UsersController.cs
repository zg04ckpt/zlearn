using Application.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System;

namespace BE.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]LoginRequest request)
        {
            var result = await _userService.Authenticate(request);
            return ApiResult(result);
        }

        private IActionResult ApiResult(ApiResult result)
        {
            if (result.Code == HttpStatusCode.OK)
                return Ok(result);

            if (result.Code == HttpStatusCode.BadRequest)
                return BadRequest(result);
            if (result.Code == HttpStatusCode.NotFound)
                return NotFound(result);
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
