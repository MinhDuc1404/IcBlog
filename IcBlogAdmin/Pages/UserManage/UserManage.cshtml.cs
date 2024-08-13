using IcBlog.Infrastructure.Data;
using IcBlog.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IcBlogAdmin.Pages.UserManage
{
    [Authorize(Roles = "admin")]
    public class UserManageModel : PageModel
    {
        private readonly BlogContext _blogContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserManageModel(BlogContext blogContext, UserManager<ApplicationUser> userManager)
        {
            _blogContext = blogContext;
            _userManager = userManager;
        }
        public IList<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

        public Dictionary<string, IList<string>> userRoles = new Dictionary<string, IList<string>>();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var applicationUsers = from m in _blogContext.ApplicationUsers
                                   select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                applicationUsers = applicationUsers.Where(m => (m.FirstName.Contains(SearchString) || m.LastName.Contains(SearchString)));
            }
            userRoles = new Dictionary<string, IList<string>>();
            ApplicationUsers = await applicationUsers.ToListAsync();

            foreach (var user in ApplicationUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var user = await _blogContext.ApplicationUsers.FindAsync(id);
            var roles = await _userManager.GetRolesAsync(user);


            if (roles.Contains("admin"))
            {
                ModelState.AddModelError(string.Empty, "Admin users cannot be deleted.");
                return RedirectToPage();
            }
            if (user != null)
            {
                _blogContext.ApplicationUsers.Remove(user);
                await _blogContext.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
