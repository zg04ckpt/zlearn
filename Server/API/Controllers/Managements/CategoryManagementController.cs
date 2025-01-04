using API.Authorization;
using Core.Common;
using Core.DTOs;
using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Managements
{
    [Route("api/managements/categories")]
    [ApiController]
    [Authorize(Consts.ADMIN_ROLE)]
    public class CategoryManagementController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryManagementController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategoriesTree()
        {
            return Ok(await _categoryService.GetCategoryTree());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO data)
        {
            return Ok(await _categoryService.CreateNewCategory(data));
        }


        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update(string categoryId, CategoryDTO data)
        {
            return Ok(await _categoryService.UpdateCategory(categoryId, data));
        }


        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(string categoryId)
        {
            return Ok(await _categoryService.DeteleCategory(categoryId));
        }
    }
}
