using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CategoryDTO
    {
        [Required(ErrorMessage = "Tên danh mục không được bỏ trống")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Slug cho danh mục không được bỏ trống")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Mô tả danh mục không được bỏ trống")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Chưa chỉ định danh mục cha")]
        public string ParentId { get; set; }

        [Required(ErrorMessage = "Chưa chỉ định đường dẫn mặc định cho danh mục")]
        public string Link { get; set; }
    }

    public class CategoryItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }

    public class CategoryBreadcrumbDTO
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class CategoryTreeNodeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public List<CategoryTreeNodeDTO> Children { get; set; } = new List<CategoryTreeNodeDTO>();
    }
}
