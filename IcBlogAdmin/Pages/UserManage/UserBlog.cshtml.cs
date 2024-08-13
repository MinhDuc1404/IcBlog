using IcBlog.Infrastructure.Data;
using IcBlog.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IcBlogAdmin.Pages.UserManage
{
    public class UserBlogModel : PageModel
    {
        private readonly BlogContext _blogContext;

        public UserBlogModel(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationUser = await _blogContext.ApplicationUsers.FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ApplicationUser = applicationUser;
            return Page();
        }
    }
}
