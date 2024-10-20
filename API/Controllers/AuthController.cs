using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            return Ok(await _authService.Login(dto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            return Ok(await _authService.Register(dto, Request.Headers.Origin.ToString()));
        }

        [HttpGet("email-confirm")]
        public async Task<IActionResult> ValidateEmail([FromQuery] string id, [FromQuery] string token)
        {
            return Ok(await _authService.ValidateEmail(id, token));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok(await _authService.Logout());
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO token)
        {
            return Ok(await _authService.RefreshToken(token));
        }
    }
}
