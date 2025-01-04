using API.Authorization;
using BE.Controllers;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Features;
using Core.Interfaces.IServices.System;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly ICategoryService _categoryService;

        public TestsController(ITestService testService, ILogger<TestsController> logger, ICategoryService categoryService)
        {
            _testService = testService;
            _categoryService = categoryService;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateTestDTO dto)
        {
            return Ok(await _testService.CreateTest(User, dto));
        }


        [HttpDelete("{testId}")]
        [Authorize]
        public async Task<IActionResult> Delete(string testId)
        {
            return Ok(await _testService.DeleteTest(User, testId));
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] TestSearchDTO dto)
        {
            return Ok(await _testService.GetAsItems(dto));
        }

        [HttpGet("all-info")]
        [Authorize(Consts.ADMIN_ROLE)]
        public async Task<IActionResult> GetAllInfos([FromQuery] TestSearchDTO dto)
        {
            return Ok(await _testService.GetAsInfos(dto));
        }


        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _categoryService.GetChildrenCategoriesBySlug("trac-nghiem"));
        }


        [HttpGet("my-tests")]
        [Authorize]
        public async Task<IActionResult> GetAllMyTests()
        {
            return Ok(await _testService.GetTestInfosOfUser(User));
        }


        [HttpGet("my-results")]
        [Authorize]
        public async Task<IActionResult> GetResultsByUserId()
        {
            return Ok(await _testService.GetTestResultsOfUser(User));
        }


        [HttpGet("{id}/content")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTestContent(string id)
        {
            return Ok(await _testService.GetTestContent(User, id));
        }


        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTestInfo(string id)
        {
            return Ok(await _testService.GetTestInfo(id));
        }


        [HttpPost("mark-test")]
        [AllowAnonymous]
        public async Task<IActionResult> MarkTest([FromBody]MarkTestDTO dto)
        {
            return Ok(await _testService.MarkTest(User, dto, HttpContext.Connection.RemoteIpAddress!.ToString()));
        }


        [HttpPut("{testId}")]
        [Authorize]
        public async Task<IActionResult> Update(string testId, [FromForm] UpdateTestDTO dto)
        {
            return Ok(await _testService.UpdateTest(User, testId, dto));
        }


        [HttpGet("{testId}/update-content")]
        [Authorize]
        public async Task<IActionResult> GetUpdateContent(string testId)
        {
            return Ok(await _testService.GetTestUpdateContent(User, testId));
        }


        [HttpPost("save")]
        [Authorize]
        public async Task<IActionResult> SaveTest([FromQuery] string testId)
        {
            return Ok(await _testService.SaveTest(User, testId));
        }


        [HttpGet("save")]
        [Authorize]
        public async Task<IActionResult> GetSavedTest()
        {
            return Ok(await _testService.GetSavedTestsOfUser(User));
        }


        [HttpGet("save/isSaved")]
        [Authorize]
        public async Task<IActionResult> IsSaved([FromQuery] string testId)
        {
            return Ok(await _testService.IsSaved(User, testId));
        }


        [HttpDelete("save")]
        [Authorize]
        public async Task<IActionResult> RemoveSavedTest([FromQuery]string testId)
        {
            return Ok(await _testService.DeleteFromSaved(User, testId));
        }
    }
}

