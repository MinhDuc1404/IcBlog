using Microsoft.AspNetCore.Mvc;

namespace IcBlog.Components
{
	public class FooterViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
