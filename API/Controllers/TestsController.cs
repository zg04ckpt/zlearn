using API.Authorization;
using Application.Features.Learn;
using BE.Controllers;
using Microsoft.AspNetCore.Mvc;
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

        public TestsController(ITestService testService, ILogger<TestsController> logger)
        {
            _testService = testService;
        }


        [HttpPost]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> Create([FromForm] CreateTestRequest request)
        {
            try
            {
                await _testService.Create(request, User);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpDelete("{testId}")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> Delete(string testId)
        {
            try
            {
                await _testService.Delete(testId, GetUserIdFromClaimPrincipal());
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

        [HttpGet("my-tests")]
        [Authorize]
        public async Task<IActionResult> GetAllMyTests()
        {
            try
            {
                return Ok(await _testService.GetTestsByUserId(GetUserIdFromClaimPrincipal()));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("results")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> GetAllResults([FromQuery] PagingRequest request)
        {
            try
            {
                return Ok(await _testService.GetAllResults(GetUserIdFromClaimPrincipal(), request));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("my-results")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> GetResultsByUserId()
        {
            try
            {
                return Ok(await _testService.GetResultsByUserId(GetUserIdFromClaimPrincipal()));
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
                return Ok(await _testService.GetTestContentById(User, id));
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
                return Ok(await _testService.MarkTest(
                    request, 
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    User));
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


        [HttpPut("{testId}")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> Update(string testId, [FromForm] TestUpdateRequest request)
        {
            try
            {
                await _testService.Update(GetUserIdFromClaimPrincipal(), testId, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("{testId}/update-content")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> GetUpdateContent(string testId)
        {
            try
            {
                return Ok(
                    await _testService.GetTestUpdateContent(GetUserIdFromClaimPrincipal(), testId));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpPost("save")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> SaveTest([FromQuery] string testId)
        {
            try
            {
                await _testService.SaveTest(GetUserIdFromClaimPrincipal(), testId);
                return Ok();
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("save")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> GetSavedTest()
        {
            try
            {
                return Ok(await _testService.GetSavedTestsByUserId(GetUserIdFromClaimPrincipal()));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("save/isSaved")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> IsSaved([FromQuery] string testId)
        {
            try
            {
                return Ok(await _testService.IsSaved(GetUserIdFromClaimPrincipal(), testId));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpDelete("save")]
        [Authorize(Consts.DEFAULT_USER_ROLE)]
        public async Task<IActionResult> RemoveSavedTest([FromQuery]string testId)
        {
            try
            {
                await _testService.DeleteFromSaved(GetUserIdFromClaimPrincipal(), testId);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

