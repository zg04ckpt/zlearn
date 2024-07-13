using Application.System.Users;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Utilities;
using ViewModels.Common;
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            return ApiResult(await _userService.Authenticate(request));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.Register(request, Request.Headers.Origin.ToString());
            return ApiResult(result);
        }

        [HttpGet("email-confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateEmail([FromQuery]string id, [FromQuery]string token)
        {
            return ApiResult(await _userService.EmailValidate(id, token));
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] Token token)
        {
            return ApiResult(await _userService.RefreshToken(token));
        }

        [HttpGet("paging")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetUsers([FromQuery]PagingRequest request)
        {
            return ApiResult(await _userService.GetUsers(request));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateRequest request)
        {
            return ApiResult(await _userService.UpdateUser(id, request));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetUser(string id)
        {
            return ApiResult(await _userService.GetUserById(id));
        }

        [HttpGet("{userId}/roles")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAllRoles(string userId)
        {
            return ApiResult(await _userService.GetAllRoles(userId));
        }

        [HttpPost("{userId}/role-assign")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> AssignRole(string userId, [FromBody] RoleAssignRequest request)
        {
            return ApiResult(await _userService.RoleAssign(userId, request));
        }
    }
}
