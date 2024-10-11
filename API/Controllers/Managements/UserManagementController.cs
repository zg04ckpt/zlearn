using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.Management;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers.Managements
{
    [Route("api/managements/users")]
    [ApiController]
    public class UserManagementController : BaseController
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers(int pageIndex, int pageSize, [FromQuery]UserManagementSearchDTO? searchDTO)
        {
            List<ExpressionFilter> filters = new List<ExpressionFilter>();
            var properties = typeof(UserManagementSearchDTO).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(searchDTO);
                if (value != null)
                {
                    filters.Add(new ExpressionFilter
                    {
                        Property = property.Name,
                        Value = value,
                        Comparison = Comparison.Contains
                    });
                }
            }

            return Ok(await _userManagementService.GetAllUsers(pageSize, pageIndex, filters));
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
