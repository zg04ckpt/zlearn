using Application.Common;
using Application.Practice;
using BE.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.QuestionSet;

namespace ZG04.BE.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsController : BaseController
    {
        private readonly ITestService _questionSetService;
        private readonly ILogger<TestsController> _logger;

        //test
        private readonly IEmailSender _emailSender;

        public TestsController(ITestService questionSetService, ILogger<TestsController> logger, IEmailSender emailSender)
        {
            _questionSetService = questionSetService;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            return ApiResult(await _questionSetService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return ApiResult(await _questionSetService.GetById(id));
        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] QSCreateRequest request)
        {
            return ApiResult(await _questionSetService.Create(request));
        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] QSUpdateRequest questionSet)
        {
            return ApiResult(await _questionSetService.Update(id, questionSet));
        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return ApiResult(await _questionSetService.Delete(id));
        }
    }
}
