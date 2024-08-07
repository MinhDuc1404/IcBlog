using IcBlog.Helper;
using IcBlog.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IcBlog.Models
{
    public class CreateBlogViewModel
    {
        public Blog Blog { get; set; }
        [Required(ErrorMessage ="Vui Lòng Chọn Ảnh")]
        public IFormFile BlogImage { get; set; }
        public string? NewCategoryName { get; set; } // For new category
    }
}
