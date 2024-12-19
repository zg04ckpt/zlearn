using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.System;
using Core.Mappers;
using Data.Entities.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.System
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<APIResult<string>> CreateNewCategory(CategoryDTO data)
        {
            // Check if name is duplicated
            if (await _categoryRepository.IsExist(e => e.Name == data.Name && e.ParentId.ToString().Equals(data.ParentId)))
            {
                throw new ErrorException("Tên danh mục đã tồn tại cùng cấp!");
            }

            // Check if slug is duplicated
            if (await _categoryRepository.IsExist(e => e.Slug == data.Slug))
            {
                throw new ErrorException("Tên rút gọn danh mục đã tồn tại!");
            }

            // Create new category
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = data.Name,
                Description = data.Description,
                ParentId = data.ParentId != null ? Guid.Parse(data.ParentId) : null,
                Slug = data.Slug
            };
            _categoryRepository.Create(category);
            await _categoryRepository.SaveChanges();

            // return new cate id
            return new APISuccessResult<string>("Tạo danh mục mới thành công!", category.Id.ToString());
        }

        public async Task<APIResult> DeteleCategory(string categoryId)
        {
            var category = await _categoryRepository.GetById(Guid.Parse(categoryId))
                ?? throw new ErrorException("Danh mục không tồn tại!");

            // check if this cate has child ? 
            if (await _categoryRepository.IsExist(e => e.ParentId.Equals(category.Id)))
            {
                throw new ErrorException("Không thể xóa danh mục này khi nó có danh mục con!");
            }

            // root cannot be removed
            if (category.ParentId is null)
            {
                throw new ErrorException("Không thể xóa danh mục gốc!");
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChanges();
            return new APISuccessResult("Xóa danh mục thành công!");
        }

        public async Task<APIResult<CategoryTreeNodeDTO>> GetCategoryTree()
        {
            var categories = (await _categoryRepository.GetAll()).ToList();

            // Get map of categories
            Dictionary<Guid, List<Category>> map = categories
                .Where(e => e.ParentId != null)
                .GroupBy(e => e.ParentId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Get root of categories tree
            var root = CategoryMapper.MapToNode(categories.First(e => e.ParentId == null));
            // Get children of root
            root.Children = GetChildrenNode(Guid.Parse(root.Id), map);
            return new APISuccessResult<CategoryTreeNodeDTO>(root);
        }

        // Recursive function for build category tree
        private static List<CategoryTreeNodeDTO> GetChildrenNode(Guid parentId, Dictionary<Guid, List<Category>> map)
        {
            var children = new List<CategoryTreeNodeDTO>();
            var categories = map.ContainsKey(parentId)? map[parentId] : new List<Category>();
            foreach (var item in categories)
            {
                var node = CategoryMapper.MapToNode(item);
                node.Children = GetChildrenNode(Guid.Parse(node.Id), map);
                children.Add(node);
            }
            return children;
        }

        public async Task<APIResult> UpdateCategory(string categoryId, CategoryDTO data)
        {
            var category = await _categoryRepository.GetById(Guid.Parse(categoryId))
                ?? throw new ErrorException("Danh mục không tồn tại!");

            // root cannot be updated
            if (category.ParentId is null)
            {
                throw new ErrorException("Không thể cập nhật danh mục gốc!");
            }

            // Check if name is duplicated
            if (await _categoryRepository.IsExist(e => e.Name == data.Name && e.ParentId.ToString().Equals(data.ParentId)))
            {
                throw new ErrorException("Tên danh mục đã tồn tại cùng cấp!");
            }

            // Check if slug is duplicated
            if (await _categoryRepository.IsExist(e => !e.Equals(category) && e.Slug == data.Slug))
            {
                throw new ErrorException("Tên rút gọn danh mục đã tồn tại!");
            }

            //update
            category.ParentId = Guid.Parse(data.ParentId);
            category.Name = data.Name;
            category.Description = data.Description;
            category.Slug = data.Slug;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChanges();
            return new APISuccessResult("Cập nhật danh mục thành công!");
        }

        public async Task<APIResult<List<CategoryItemDTO>>> GetChildrenCategoriesBySlug(string slug)
        {
            var parentId = (await _categoryRepository.Get(e => e.Slug.Equals(slug)))?.Id
                ?? throw new ErrorException("Danh mục không tồn tại");
            var categories = (await _categoryRepository
                .GetAll(e => e.ParentId != null && e.ParentId.Equals(parentId)))
                .Select(e => CategoryMapper.MapToItem(e))
                .ToList();
            return new APISuccessResult<List<CategoryItemDTO>>(categories);
        }
    }
}
