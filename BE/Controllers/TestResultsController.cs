using Application.Practice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ViewModels.Test;

namespace BE.Controllers
{
    [Route("api/test-results")]
    [ApiController]
    public class TestResultsController : ControllerBase
    {
        private readonly ITestResultService _testResultService;

        public TestResultsController(ITestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _testResultService.GetAll();
            if (result.Code == System.Net.HttpStatusCode.InternalServerError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TestResultCreateRequest request)
        {
            var result = await _testResultService.Create(request);
            if (result.Code == System.Net.HttpStatusCode.InternalServerError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }
    }
}
