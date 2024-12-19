using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.Management;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers.Managements
{
    [Route("api/managements/users")]
    [ApiController]
    [Authorize(Consts.ADMIN_ROLE)]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery]UserSearchDTO data)
        {
            return Ok(await _userManagementService.GetAllUsers(data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok(await _userManagementService.GetUserById(id));
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetRolesOfUser(string id)
        {
            return Ok(await _userManagementService.GetAllRolesOfUser(id));
        }

        [HttpPost("{id}/assign-role")]
        public async Task<IActionResult> AssignRole(string id, [FromBody] RoleAssignmentDTO dto)
        {
            return Ok(await _userManagementService.AssignRole(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return Ok(await _userManagementService.DeleteUser(id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserManagementDTO dto)
        {
            return Ok(await _userManagementService.UpdateUser(dto));
        }
    }
}
