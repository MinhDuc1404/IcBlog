using IcBlog.Infrastructure.Models;
using IcBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IcBlog.Services.Interface
{
    public interface IGetBlogRepo
    {
        
        Task<BlogIndexViewModel> GetBlogIndex(int? category);
        Task<BlogDetailsViewModel> GetBlogDetail(int id);

        Task<SearchViewModel> SearchBlog(string searching);

        Task<Blog> CreateBlog(CreateBlogViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);

        Task<UpdateBlogViewModel> GetEditViewModel(int id);

        Task<UpdateBlogViewModel> UpdatePost(UpdateBlogViewModel editViewModel, ClaimsPrincipal claimsPrincipal);

        Task<BlogDetailsViewModel> CreateComment(BlogDetailsViewModel blogDetails, ClaimsPrincipal claimsPrincipal);



    }
}
