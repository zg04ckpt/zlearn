using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("breadcrumb")]
        public async Task<IActionResult> GetCategoryAsBreadcrumb(string currentSlug)
        {
            return Ok(await _categoryService.GetBreadcrumb(currentSlug));
        }
    }
}
