using IcBlog.Infrastructure.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IcBlog.Infrastructure.Services.Interface
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    }
}
