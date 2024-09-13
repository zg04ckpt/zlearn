using API.Authorization;
using Application.System.Roles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Utilities;
using ViewModels.System.Roles;

namespace BE.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }




        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _roleService.GetAll());
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }




        [HttpPost]
        [Authorize(Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> CreateRole([FromBody]RoleCreateRequest request)
        {
            try
            {
                await _roleService.Add(request.Name, request.Description);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }



        [HttpPut]
        [Authorize(Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> UpdateRole([FromBody]RoleModel role)
        {
            try
            {
                await _roleService.Update(role);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }



        [HttpDelete("{id}")]
        [Authorize(Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _roleService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
