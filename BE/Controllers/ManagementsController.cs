using Application.System.Manage;
using BE.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using Utilities;
using ViewModels.Common;
using ViewModels.System.Manage;

namespace BE.Controllers
{
    [Route("api/managements")]
    [ApiController]
    public class ManagementsController : BaseController
    {
        private readonly IAdminService _adminService;

        public ManagementsController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        [HttpGet("users")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers([FromQuery]PagingRequest request)
        {
            try
            {
                return Ok(await _adminService.GetAllUsers(request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("users/filter-by-username/{key}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAllUsersByUserName(string key, [FromQuery] PagingRequest request)
        {
            try
            {
                return Ok(await _adminService.FindByUserName(key, request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("users/filter-by-email/{key}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAllUsersByEmail(string key, [FromQuery] PagingRequest request)
        {
            try
            {
                return Ok(await _adminService.FindByEmail(key, request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("users/filter-by-name/{key}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAllUsersByName(string key, [FromQuery] PagingRequest request)
        {
            try
            {
                return Ok(await _adminService.FindByName(key, request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("users/filter-by-phone-num/{key}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAllUsersByPhoneNum(string key, [FromQuery] PagingRequest request)
        {
            try
            {
                return Ok(await _adminService.FindByPhoneNum(key, request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("users/filter-by-role/{key}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAllUsersByRole(string key, [FromQuery] PagingRequest request)
        {
            try
            {
                return Ok(await _adminService.FindByRole(key, request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("users/{id}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                return Ok(await _adminService.GetUserById(id));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpDelete("users/{id}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _adminService.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }



        [HttpPut("users")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> UpdateUser([FromBody] UserManagementModel user)
        {
            try
            {
                await _adminService.UpdateUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [Authorize()]
        [HttpGet("users/{id}/roles")]
        public async Task<IActionResult> GetAllRolesOfUser(string id)
        {
            try
            {
                return Ok(await _adminService.GetAllRolesOfUser(id));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }



        [HttpPost("users/{id}/assign-role")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> AssignRole(string id, [FromBody]RoleAssignRequest request)
        {
            try
            {
                await _adminService.AssignRole(id, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
