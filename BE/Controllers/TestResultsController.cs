using Application.Practice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Utilities;
using ViewModels.Common;
using ViewModels.Test;

namespace BE.Controllers
{
    [Route("api/test-results")]
    [ApiController]
    public class TestResultsController : BaseController
    {
        private readonly ITestResultService _testResultService;
        private readonly ILogger<TestResultsController> _logger;

        public TestResultsController(ITestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        [HttpGet]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _testResultService.GetAll();
            return ApiResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TestResultCreateRequest request)
        {
            var result = await _testResultService.Create(request, HttpContext.Connection);
            return ApiResult(result);
        }

        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var result = await _testResultService.RemoveAll();
            return ApiResult(result);
        }
    }
}
