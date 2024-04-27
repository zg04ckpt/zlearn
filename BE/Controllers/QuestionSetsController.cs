using Application.Practice;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.QuestionSet;

namespace ZG04.BE.Controllers
{
    [Route("api/question-sets")]
    [ApiController]
    public class QuestionSetsController : ControllerBase
    {
        private readonly IQuestionSetService _questionSetService;
        private readonly ILogger<QuestionSetsController> _logger;

        public QuestionSetsController(IQuestionSetService questionSetService, ILogger<QuestionSetsController> logger)
        {
            _questionSetService = questionSetService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _questionSetService.GetAll();
            return ApiResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _questionSetService.GetById(id);
            return ApiResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] QSCreateRequest request)
        {
            var result = await _questionSetService.Create(request);
            return ApiResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] QSUpdateRequest questionSet)
        {
            var result = await _questionSetService.Update(id, questionSet);
            return ApiResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _questionSetService.Delete(id);
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
