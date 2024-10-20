using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut("{id}/profile")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> UpdateUserDetail(string id, [FromBody] UserProfileDTO dto)
        {
            return Ok(await _userService.UpdateProfile(id, dto));
        }

        [HttpGet("{id}/profile")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> GetUserDetail(string id)
        {
            return Ok(await _userService.GetProfile(id));
        }
    }
}
