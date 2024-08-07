using IcBlog.Infrastructure.Models;

namespace IcBlog.Models
{
    public class UserAccountViewModel
    {
        public ApplicationUser UserAccount { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
