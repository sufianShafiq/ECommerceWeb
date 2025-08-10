using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    /// <summary>
    /// Provides basic landing pages for the site. At the moment this controller
    /// contains only an index action but can be expanded later.
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}