using Application.System.Users;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
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

        [HttpGet("paging")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetUsers([FromQuery]PagingRequest request)
        {
            try
            {
                return Ok(await _userService.GetUsers(request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/profile")]
        [Authorize(Roles = Consts.DEFAULT_USER_ROLE)]
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

        [HttpGet("{id}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                return Ok(await _userService.GetUserById(id));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}/profile")]
        [Authorize(Roles = Consts.DEFAULT_USER_ROLE)]
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

        [HttpGet("{userId}/roles")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAllRoles(string userId)
        {
            try
            {
                return Ok(await _userService.GetAllRoles(userId));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("{userId}/role-assign")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> AssignRole(string userId, [FromBody] RoleAssignRequest request)
        {
            try
            {
                await _userService.RoleAssign(userId, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
