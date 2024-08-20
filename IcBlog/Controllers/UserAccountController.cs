using Microsoft.AspNetCore.Mvc;
using IcBlog.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using IcBlog.Infrastructure.Models;
using IcBlog.Models;
using IcBlog.Helper;
using IcBlog.Services;

namespace IcBlog.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBlogServices _blogServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly IProfilePictureService _profilePictureService;
        public  UserAccountController(IUserService userService, UserManager<ApplicationUser> userManager, IBlogServices blogServices, IWebHostEnvironment webHostEnvironment, HttpClient httpClient, IProfilePictureService profilePictureService)
        {
            _userService = userService;
            _userManager = userManager;
            _blogServices = blogServices;
            _webHostEnvironment = webHostEnvironment;
            _httpClient = httpClient;
            _profilePictureService = profilePictureService;
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
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not logged in
            }

            var user = await _userService.GetUserByIdAsync(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account"); // Redirect to login if not logged in
                }
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(); 
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;

                var result = await _userService.UpdateUserAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeAvatar(IFormFile avatarUpload)
        {
            if (avatarUpload != null && avatarUpload.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Call service to save the profile picture
                    await _profilePictureService.SaveProfilePictureAsync(user.Id, avatarUpload);

                    // Optionally: Display a success message
                    TempData["SuccessMessage"] = "Profile picture updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "User not found.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid image upload.";
            }

            return RedirectToAction("Index"); // Redirect to the appropriate view
        }



    }
}
