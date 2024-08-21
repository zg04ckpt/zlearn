using Application.Common;
using Application.Features.Learn;
using BE.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Threading.Tasks;
using Utilities;
using ViewModels.Features.Learn.Test;

namespace ZG04.BE.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsController : BaseController
    {
        private readonly ITestService _testService;
        private readonly ILogger<TestsController> _logger;

        public TestsController(ITestService testService, ILogger<TestsController> logger)
        {
            _testService = testService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _testService.GetAll());
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                return Ok(await _testService.GetById(id));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestRequest request)
        {
            try
            {
                await _testService.Create(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TestUpdateRequest questionSet)
        {
            try
            {
                await _testService.Update(id, questionSet);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _testService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}/questions")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllQuestionsByTestId(string id)
        {
            try
            {
                return Ok(await _testService.GetAllQuestionByTestId(id));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("results")]
        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        public async Task<IActionResult> GetAllResults()
        {
            try
            {
                return Ok(await _testService.GetAllResults());
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("results")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveResult([FromBody] SaveTestResultRequest request)
        {
            try
            {
                await _testService.SaveResult(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [Authorize(Roles = Consts.DEFAULT_ADMIN_ROLE)]
        [HttpDelete("results")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                await _testService.RemoveAllResults();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

