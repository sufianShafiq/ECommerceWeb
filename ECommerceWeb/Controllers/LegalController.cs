using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    public class LegalController : Controller
    {
        [HttpGet("/privacy")]
        public IActionResult Privacy() => View(); // Views/Legal/Privacy.cshtml

        [HttpGet("/facebook/data-deletion")]
        public IActionResult FacebookDataDeletion() => View(); // Views/Legal/FacebookDataDeletion.cshtml
    }
}
