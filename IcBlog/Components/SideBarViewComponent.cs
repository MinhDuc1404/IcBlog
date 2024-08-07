using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using IcBlog.Infrastructure.Data;
using IcBlog.Infrastructure.Models;
using IcBlog.Infrastructure.Services.Interface;
using IcBlog.Models;
namespace BlogMVC.Components
{
       public class SideBarViewComponent : ViewComponent
    {
        private readonly BlogContext _context;
        private readonly IBlogServices _blogServices;
        private readonly ICategoryServices _categoryServices;
        public SideBarViewComponent(BlogContext context, IBlogServices blogServices, ICategoryServices categoryServices)
        {
            _context = context;
            _blogServices = blogServices;
            _categoryServices = categoryServices;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blog = await _blogServices.GetListBlogAsync();
            var categories = await _categoryServices.GetListCategoryAsync();

         return View(new SideBarViewModel
         {
             Blogs = blog,
             Categories = categories
         });
        }
    }
}
