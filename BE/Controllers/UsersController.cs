using Application.System;
using Data.Entities;
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
    public class UsersController : BaseController
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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm]RegisterRequest request)
        {
            var result = await _userService.Register(request, Request.Host.Value, Request.Scheme);
            return ApiResult(result);
        }

        [HttpPost("email-confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateEmail([FromQuery]string id, [FromQuery]string token)
        {
            var result = await _userService.EmailValidate(id, token);
            return ApiResult(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromForm]Token token)
        {
            var result = await _userService.RefreshToken(token);
            return ApiResult(result);
        }
    }
}
