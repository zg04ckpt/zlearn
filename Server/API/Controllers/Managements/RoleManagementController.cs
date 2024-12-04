using API.Authorization;
using Core.DTOs;
using Core.Interfaces.IServices.Management;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Managements
{
    [Route("api/managements/roles")]
    [ApiController]
    [Authorize(roles: "Admin")]
    public class RoleManagementController : ControllerBase
    {
        private readonly IRoleManagementService _roleManagementService;

        public RoleManagementController(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roleManagementService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDTO dto)
        {
            return Ok(await _roleManagementService.Add(dto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(RoleDTO dto)
        {
            return Ok(await _roleManagementService.Update(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _roleManagementService.Delete(id));
        }
    }
}
