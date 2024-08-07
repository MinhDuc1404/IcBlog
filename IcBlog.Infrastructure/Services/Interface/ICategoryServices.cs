using IcBlog.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcBlog.Infrastructure.Services.Interface
{
    public interface ICategoryServices
    {
        Task<List<Category>> GetListCategoryAsync();

        Task<List<SelectListItem>> GetSelectListCategoryAsync();

        Task<Category> AddCategory(Category category);

        Task<bool> DoesCategoryExistAsync(string categoryName);
    }
}
