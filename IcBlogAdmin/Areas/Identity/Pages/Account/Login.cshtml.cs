using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using IcBlog.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using IcBlog.Infrastructure.Data;

namespace IcBlogAdmin.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly BlogContext _blogContext;
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger, BlogContext blogContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _blogContext = blogContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                // Get the user from the database
                var user = await _userManager.FindByEmailAsync(Input.Email);

                // If login is successful, increment the login count
                if (result.Succeeded && user != null)
                {
                    // Increment login count for the user
                    user.LoginCount++;
                    _blogContext.ApplicationUsers.Update(user); // Update the user in the database
                    await _blogContext.SaveChangesAsync(); // Save the changes to the database

                    // Log the login attempt
                    var loginAttempt = new LoginAttempt
                    {
                        UserId = user.Id,
                        AttemptedAt = DateTime.UtcNow,
                        Success = true
                    };
                    _blogContext.LoginAttempts.Add(loginAttempt);
                    await _blogContext.SaveChangesAsync();

                    _logger.LogInformation("User logged in successfully.");

                    // Redirect based on role
                    if (await _userManager.IsInRoleAsync(user, "admin"))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        _logger.LogWarning("Non-admin user attempted to log in.");
                        await _signInManager.SignOutAsync();
                        ErrorMessage = "You are not an admin.";
                        return RedirectToPage("./Login");
                    }
                }

                // Handle login failures
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed; redisplay form
            return Page();
        }

    }
}
