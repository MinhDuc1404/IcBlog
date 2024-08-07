using IcBlog.Infrastructure.Models;
using System.Threading.Tasks;

namespace IcBlog.Infrastructure.Services.Interface
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
