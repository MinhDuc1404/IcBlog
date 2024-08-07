using IcBlog.Infrastructure.Services.Interface;
using IcBlog.Models;
using IcBlog.Services.Interface;

namespace IcBlog.Services
{
    public class GetHomeRepo : IGetHomeRepo
    {
        private readonly IBlogServices _blogServices;

        public GetHomeRepo(IBlogServices blogServices)
        {
            _blogServices = blogServices;
        }

        public async Task<HomeViewModel> GetHomeIndex()
        {
            var blog = await _blogServices.GetListBlogAsync();

            return new HomeViewModel
            {
                Blogs = blog
            };

        }
    }
}
