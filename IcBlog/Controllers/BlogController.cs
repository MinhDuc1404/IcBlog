using IcBlog.Infrastructure.Services.Interface;
using IcBlog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using IcBlog.Models;
using Microsoft.AspNetCore.Identity;
using IcBlog.Infrastructure.Services;
using IcBlog.Infrastructure.Models;

namespace IcBlog.Controllers
{
	public class BlogController : Controller
	{
        private readonly IGetBlogRepo _blogRepo;
        private readonly ICategoryServices _categoryServices;
   
		public BlogController(IGetBlogRepo blogRepo, ICategoryServices categoryServices)
        {
            _blogRepo = blogRepo;
            _categoryServices = categoryServices;
        }

        [Route("blog.html/{category:int?}")]
        public async Task<IActionResult> Index(int? category)
		{
          return View( await _blogRepo.GetBlogIndex(category));
        }
        [Route("blog-details.html/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _blogRepo.GetBlogDetail(id));
        }
 
        public async Task<IActionResult> Create()
        {
            var category = await _categoryServices.GetSelectListCategoryAsync();
            ViewBag.Categories = category;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogViewModel createViewModel)
        {
            // Kiểm tra nội dung nhập vào
            var listcategory = await _categoryServices.GetSelectListCategoryAsync();
            if (ModelState["Blog.Content"].Errors.Count > 0)
            {
                ViewBag.Categories = listcategory;
                return View(createViewModel);
            }


            // Kiểm tra tính hợp lệ của category

            if (createViewModel.NewCategoryName != null)
            {
                var categoryExists = await _categoryServices.DoesCategoryExistAsync(createViewModel.NewCategoryName);
                if (categoryExists)
                {
                    ViewData["CategoryExist"] = "Danh mục đã có sẵn vui lòng tạo mới danh mục khác";
                    ViewBag.Categories = listcategory;
                    return View(createViewModel);

                }
                    Category category = new Category();
                    category.Name = createViewModel.NewCategoryName;
                    await _categoryServices.AddCategory(category);
                    createViewModel.Blog.CategoryID = category.CategoryID;
               
            }

            // Kiểm tra tạo mới danh mục hoặc chọn danh mục 

            if ((createViewModel.Blog.CategoryID == null) && (createViewModel.NewCategoryName == null))
                {
                    ViewData["CategoryID"] = "Vui lòng chọn danh mục hoặc tạo danh mục mới";
                    ViewBag.Categories = listcategory;
                    return View(createViewModel);
                }
            
            await _blogRepo.CreateBlog(createViewModel, User);

                return RedirectToAction("Index");
            }
            
        

        public async Task<IActionResult> Search(string searching)
        {
            return View(await _blogRepo.SearchBlog(searching));
        }
        public async Task<IActionResult> Update(int id)
        {

            return View(await _blogRepo.GetEditViewModel(id));
        }
        [HttpPost]
        public async Task<IActionResult> Comment(BlogDetailsViewModel blogDetails)
        {
            var result = await _blogRepo.CreateComment(blogDetails,User);
            if (result == null)
            {
                return RedirectToAction("Details", new { blogDetails.Blog.BlogID });
            }
            return RedirectToAction("Details", new { id = result.Blog.BlogID });
        }
    }
}
