using ZG04WEB.Application.System;
using ZG04WEB.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ZG04WEB.ViewModels.System;

namespace ZG04.BE.Controllers
{
    [Route("api/question-sets")]
    [ApiController]
    public class QuestionSetsController : ControllerBase
    {
        private readonly IQuestionSetService _questionSetService;

        public QuestionSetsController(IQuestionSetService questionSetService)
        {
            _questionSetService = questionSetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _questionSetService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _questionSetService.GetById(id);
            if(result.IsSuccess) return Ok(result);
            return NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionSetRequest request)
        {
            var result = await _questionSetService.Create(request);
            if(result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] QuestionSetRequest questionSet)
        {
            var result = await _questionSetService.Update(id, questionSet);
            if(result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _questionSetService.Delete(id);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
