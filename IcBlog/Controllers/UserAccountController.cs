using Microsoft.AspNetCore.Mvc;
using IcBlog.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using IcBlog.Infrastructure.Models;
using IcBlog.Models;
namespace IcBlog.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBlogServices _blogServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public  UserAccountController(IUserService userService, UserManager<ApplicationUser> userManager, IBlogServices blogServices)
        {
            _userService = userService;
            _userManager = userManager;
            _blogServices = blogServices;
        }
        [Route("user-info")]
        public async Task<IActionResult> Index()
        {
            UserAccountViewModel UserAccount = new UserAccountViewModel();
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not logged in
            }

            var user = await _userService.GetUserByIdAsync(userId);
            UserAccount.UserAccount = user;
            var blog = await _blogServices.GetblogByUserIDAsync(userId);
            UserAccount.Blogs = blog;
            return View(UserAccount);
        }
    }
}
