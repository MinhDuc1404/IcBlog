using IcBlog.Infrastructure.Data;
using IcBlog.Infrastructure.Models;
using IcBlog.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IcBlog.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly BlogContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(BlogContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
