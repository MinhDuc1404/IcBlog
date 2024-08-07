using Microsoft.AspNetCore.Mvc;

namespace IcBlog.Components
{
    public class UserSideBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        { 
            return View(); 
        }
    }
}
