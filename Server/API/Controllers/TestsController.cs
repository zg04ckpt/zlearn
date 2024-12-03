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
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> Create([FromForm] CreateTestDTO dto)
        {
            return Ok(await _testService.CreateTest(User, dto));
        }


        [HttpDelete("{testId}")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> Delete(string testId)
        {
            return Ok(await _testService.DeleteTest(User, testId));
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int pageIndex, int pageSize, [FromQuery] TestSearchDTO dto)
        {
            List<ExpressionFilter> filters = new();
            var properties = typeof(TestSearchDTO).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                if(value != null)
                {
                    filters.Add(new ExpressionFilter
                    {
                        Property = property.Name,
                        Value = value,
                        Comparison = Comparison.Contains
                    });
                }
            }

            return Ok(await _testService.SearchTest(pageIndex, pageSize, dto));
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


        [HttpGet("results")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> GetAllResults(int pageIndex, int pageSize, [FromQuery] TestResultSearchDTO dto)
        {
            List<ExpressionFilter> filters = new List<ExpressionFilter>();
            var properties = typeof(TestSearchDTO).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                if (value != null)
                {
                    filters.Add(new ExpressionFilter
                    {
                        Property = property.Name,
                        Value = value,
                        Comparison = Comparison.Contains
                    });
                }
            }

            return Ok(await _testService.GetAllResults(pageSize, pageIndex, filters));
        }


        [HttpGet("my-results")]
        [Authorize(Consts.USER_ROLE)]
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
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> Update(string testId, [FromForm] UpdateTestDTO dto)
        {
            return Ok(await _testService.UpdateTest(User, testId, dto));
        }


        [HttpGet("{testId}/update-content")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> GetUpdateContent(string testId)
        {
            return Ok(await _testService.GetTestUpdateContent(User, testId));
        }


        [HttpPost("save")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> SaveTest([FromQuery] string testId)
        {
            return Ok(await _testService.SaveTest(User, testId));
        }


        [HttpGet("save")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> GetSavedTest()
        {
            return Ok(await _testService.GetSavedTestsOfUser(User));
        }


        [HttpGet("save/isSaved")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> IsSaved([FromQuery] string testId)
        {
            return Ok(await _testService.IsSaved(User, testId));
        }


        [HttpDelete("save")]
        [Authorize(Consts.USER_ROLE)]
        public async Task<IActionResult> RemoveSavedTest([FromQuery]string testId)
        {
            return Ok(await _testService.DeleteFromSaved(User, testId));
        }


    }
}

