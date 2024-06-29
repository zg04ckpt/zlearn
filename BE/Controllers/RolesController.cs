using Application.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

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
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAll();
            return ApiResult(result);
        }

        [HttpPost]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> CreateRole([FromForm]string name, [FromForm]string desc)
        {
            var result = await _roleService.Add(name, desc);
            return ApiResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> UpdateRole(string id, [FromForm] string name, [FromForm] string desc)
        {
            var result = await _roleService.Update(id, name, desc);
            return ApiResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleService.Delete(id);
            return ApiResult(result);
        }
    }
}
