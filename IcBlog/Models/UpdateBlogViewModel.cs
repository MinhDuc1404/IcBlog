using IcBlog.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace IcBlog.Models
{
    public class UpdateBlogViewModel
    {
        public Blog Blog { get; set; }
        [Required(ErrorMessage = "Vui Lòng Chọn Ảnh")]
        public IFormFile BlogImage { get; set; }
  


    }
}
