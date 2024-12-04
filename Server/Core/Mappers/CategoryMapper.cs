using Core.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mappers
{
    public class CategoryMapper
    {
        public static CategoryTreeNodeDTO MapToNode(Category category)
        {
            return new CategoryTreeNodeDTO
            {
                Id = category.Id.ToString(),
                Name = category.Name,
                Description = category.Description,
                Slug = category.Slug,
            };
        }

        public static CategoryItemDTO MapToItem(Category category)
        {
            return new CategoryItemDTO
            {
                Id = category.Id.ToString(),
                Name = category.Name,
                Description = category.Description,
                Slug = category.Slug,
            };
        }
    }
}
