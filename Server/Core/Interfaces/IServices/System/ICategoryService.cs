using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.System
{
    public interface ICategoryService
    {
        Task<APIResult<List<CategoryItemDTO>>> GetChildrenCategoriesBySlug(string slug);
        Task<APIResult<CategoryTreeNodeDTO>> GetCategoryTree();
        Task<APIResult<List<CategoryBreadcrumbDTO>>> GetBreadcrumb(string currentCategorySlug);
        Task<APIResult<string>> CreateNewCategory(CategoryDTO data);
        Task<APIResult> UpdateCategory(string categoryId, CategoryDTO data);
        Task<APIResult> DeteleCategory(string categoryId);
    }
}
