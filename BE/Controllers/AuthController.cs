using Application.System.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using ViewModels.System.Auth;

namespace BE.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                return Ok(await _authService.Login(request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _authService.Register(request, Request.Headers.Origin.ToString());
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("email-confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateEmail([FromQuery] string id, [FromQuery] string token)
        {
            try
            {
                await _authService.EmailValidate(id, token);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("logout")]
        [Authorize(Roles = Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.Logout();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] Token token)
        {
            try
            {
                return Ok(await _authService.RefreshToken(token));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
