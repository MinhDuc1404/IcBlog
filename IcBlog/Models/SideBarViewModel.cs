using IcBlog.Infrastructure.Models;

namespace IcBlog.Models
{
    public class SideBarViewModel
    {
        public List<Blog> Blogs { get; set; }

        public List<Category> Categories { get; set;}
    }
}
