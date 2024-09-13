using API.Authorization;
using Application.System.Users;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using ViewModels.System.Users;

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
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> UpdateUserDetail(string id, [FromBody] UserDetailModel request)
        {
            try
            {
                await _userService.UpdateUserDetail(id, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}/profile")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> GetUserDetail(string id)
        {
            try
            {
                return Ok(await _userService.GetUserDetail(id));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
