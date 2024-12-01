using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            return Ok(await _userService.GetOtherUserProfile(User, userId));
        }

        [HttpPut("my-profile")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> UpdateUserDetail([FromForm] UserProfileDTO dto)
        {
            return Ok(await _userService.UpdateMyProfile(User, dto));
        }

        [HttpGet("my-profile")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> GetUserDetail()
        {
            return Ok(await _userService.GetMyProfile(User));
        }

        [HttpGet("like")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> Like(string userId)
        {
            return Ok(await _userService.Like(User, userId));
        }
    }
}
