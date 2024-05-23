using Application.Practice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using ViewModels.Common;

namespace BE.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionServices _services;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(IQuestionServices services)
        {
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByQuestionSetId(string id)
        {
            var result = await _services.GetByQuestionSetId(id);
            return ApiResult(result);
        }

        private IActionResult ApiResult(ApiResult result)
        {
            if (result.Code == HttpStatusCode.OK)
                return Ok(result);

            _logger.LogError(result.Message);
            if (result.Code == HttpStatusCode.BadRequest)
                return BadRequest(result);
            if (result.Code == HttpStatusCode.NotFound)
                return NotFound(result);
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
