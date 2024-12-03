using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.Management;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Managements
{
    [Route("api/managements/tests")]
    [ApiController]
    [Authorize(Consts.ADMIN_ROLE)]
    public class TestManagementController : ControllerBase
    {
        private readonly ITestManagementService _testManagementService;

        public TestManagementController(ITestManagementService testManagementService)
        {
            _testManagementService = testManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex, int pageSize, [FromQuery] TestSearchDTO dto)
        {
            List<ExpressionFilter> filters = new();
            var properties = typeof(TestSearchDTO).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
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

            return Ok(await _testManagementService.GetAllTest(pageIndex, pageSize, filters));
        }
    }
}
