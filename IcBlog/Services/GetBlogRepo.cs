using IcBlog.Infrastructure.Models;
using IcBlog.Infrastructure.Services.Interface;
using IcBlog.Models;
using IcBlog.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using IcBlog.Helper;
using Microsoft.AspNetCore.Authorization;

namespace IcBlog.Services
{
    public class GetBlogRepo : IGetBlogRepo
    {
        private readonly IBlogServices _blogServices;
        private readonly ICategoryServices _categoryServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GetBlogRepo(IBlogServices blogServices, UserManager<ApplicationUser> userManager, ICategoryServices categoryServices, IWebHostEnvironment webHostEnvironment)
        {
            _blogServices = blogServices;
            _userManager = userManager;
            _categoryServices = categoryServices;
            _webHostEnvironment = webHostEnvironment;

        }
      
        public async Task<BlogIndexViewModel> GetBlogIndex(int? category)
        {
            if (category == null)
            {
                var blog = await _blogServices.GetListBlogAsync();
            
                return new BlogIndexViewModel
                {
                    Blogs = blog
                 };
            }
            else
            {
                var blog = await _blogServices.GetBlogWithCateAsync(category);
              
                return new BlogIndexViewModel
                {
                    Blogs = blog
                };
            }
           
        }
        public async Task<BlogDetailsViewModel> GetBlogDetail(int id)
        {
            var blog = await _blogServices.GetBlogAsync(id);
            return new BlogDetailsViewModel
            {
                Blog = blog
            };
        }
       public async Task<SearchViewModel> SearchBlog(string searching)
        {
            var blog = await _blogServices.GetBlogSearchingAsync(searching);
       
            return new SearchViewModel
            {
                Blogs = blog
            };
        }
        public async Task<Blog> CreateBlog(CreateBlogViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal)
        {
       
            var blog = createBlogViewModel.Blog;
             blog.Author = await _userManager.GetUserAsync(claimsPrincipal);
            blog.DateTime = DateTime.Now;
            blog = await _blogServices.AddBlog(blog);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string pathToImage = Path.Combine(webRootPath, "UserFiles", $"{AppUtilities.LocDau(blog.Author.FirstName)}{AppUtilities.LocDau(blog.Author.LastName)}", blog.BlogID.ToString(), "BlogImage.jpg");

            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await createBlogViewModel.BlogImage.CopyToAsync(fileStream);
            }
            return blog;
        }
        public async Task<UpdateBlogViewModel> UpdatePost(UpdateBlogViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var blog = await _blogServices.GetBlogAsync(editViewModel.Blog.BlogID);

            blog.Title = editViewModel.Blog.Title;
            blog.Content = editViewModel.Blog.Content;
            blog.DateTime = DateTime.Now;

            if (editViewModel.BlogImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string pathToImage = Path.Combine(webRootPath, "UserFiles", $"{AppUtilities.LocDau(blog.Author.FirstName)}{AppUtilities.LocDau(blog.Author.LastName)}", blog.BlogID.ToString(), "BlogImage.jpg");

                EnsureFolder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await editViewModel.BlogImage.CopyToAsync(fileStream);
                }
            }

            return new UpdateBlogViewModel
            {
                Blog = await _blogServices.UpdateBlog(blog)
            };
        }
        public async Task<UpdateBlogViewModel> GetEditViewModel(int id)
        {
            var BlogId = id;

            var blog = await _blogServices.GetBlogAsync(BlogId);

            return new UpdateBlogViewModel
            {
                Blog = blog
            };
        }
        public async Task<BlogDetailsViewModel> CreateComment(BlogDetailsViewModel blogDetails, ClaimsPrincipal claimsPrincipal)
        {
          

            var blog = await _blogServices.GetBlogAsync(blogDetails.Blog.BlogID);

            var comment = blogDetails.Comment;
            comment.Author = await _userManager.GetUserAsync(claimsPrincipal);
            comment.CreatedOn = DateTime.Now;
            comment.Blog = blog;

            if (comment.CommentParent != null)
            {
                comment.CommentParent = await _blogServices.GetCommentAsync(comment.CommentParent.Id);
            }    
            await _blogServices.AddComment(comment);
  

            return new BlogDetailsViewModel
            {
                Comment = comment,
                Blog = blog
           
                
            };
          
        }
        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(directoryName);
            }
        }

    }
}
