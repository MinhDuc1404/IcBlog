using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcBlog.Infrastructure.Models
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }

        public ApplicationUser Author { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tiêu đề")]
        [StringLength(50,ErrorMessage ="Độ dài tiêu đề không vượt quá  50 ký tự")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        [StringLength(int.MaxValue, MinimumLength = 200, ErrorMessage = "Độ dài nội dung phải từ 200 ký tự trở lên.")]
        public string Content { get; set; }

        public DateTime? DateTime { get; set; }
        public int? CategoryID { get; set; }

        public virtual Category Category { get; set; }

        public virtual List<Comment> Comments { get; set; }
    }
}
