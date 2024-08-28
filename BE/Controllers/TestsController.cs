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
using ViewModels.Common;
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


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateTestRequest request)
        {
            try
            {
                await _testService.Create(request);
                return Ok();
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }


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


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            try
            {
                return Ok(await _testService.GetAll(request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        
        [HttpGet("results")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllResults([FromQuery] PagingRequest request)
        {
            try
            {
                return Ok(await _testService.GetAllResults(request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("{id}/content")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTestContent(string id)
        {
            try
            {
                return Ok(await _testService.GetTestContentById(id));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTestDetail(string id)
        {
            try
            {
                return Ok(await _testService.GetDetailById(id));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpPost("mark-test")]
        [AllowAnonymous]
        public async Task<IActionResult> MarkTest([FromBody]MarkTestRequest request)
        {
            try
            {
                return Ok(await _testService.MarkTest(request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpDelete("results")]
        public async Task<IActionResult> RemoveAllResults()
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


        [HttpPost("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]TestUpdateRequest request)
        {
            try
            {
                await _testService.Update(id, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

