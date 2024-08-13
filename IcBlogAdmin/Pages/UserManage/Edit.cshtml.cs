using IcBlog.Infrastructure.Data;
using IcBlog.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace IcBlogAdmin.Pages.UserManage
{
    public class EditModel : PageModel
    {
        private readonly BlogContext _blogContext;

        public EditModel(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public IList<Blog> Blogs { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the user
            var applicationUser = await _blogContext.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            ApplicationUser = applicationUser;

            // Fetch blogs, including category details
            Blogs = await _blogContext.Blogs
                .Where(u => u.Author == applicationUser)
                .Include(b => b.Category)
                .ToListAsync();

            return Page();
        }

            public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                // Collecting the errors from ModelState
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Creating a detailed error message
                var errorMessage = string.Join("; ", errors);

                // Throwing an exception with the error details
              throw new InvalidOperationException($"Model validation failed: {errorMessage}");
            }

            var user = await _blogContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); // Handle user not found
            }

            user.FirstName = ApplicationUser.FirstName;
            user.LastName = ApplicationUser.LastName;
            user.Email = ApplicationUser.Email;
            user.PhoneNumber = ApplicationUser.PhoneNumber;
            user.Address = ApplicationUser.Address;
            await _blogContext.SaveChangesAsync();

            return RedirectToPage("./UserManage");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int BlogId, string id)
        {
            if (BlogId == null)
            {
                return NotFound();
            }
            var blog = await _blogContext.Blogs.FindAsync(BlogId);

            if (blog != null)
            {
                _blogContext.Blogs.Remove(blog);
                await _blogContext.SaveChangesAsync();
            }
            return RedirectToPage("./Edit", new { id });
        }


    }
}