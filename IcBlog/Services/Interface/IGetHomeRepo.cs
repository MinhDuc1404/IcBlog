using IcBlog.Models;

namespace IcBlog.Services.Interface
{
    public interface IGetHomeRepo
    {
        Task<HomeViewModel> GetHomeIndex();
    }
}
