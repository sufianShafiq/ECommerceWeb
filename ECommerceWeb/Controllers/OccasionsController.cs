using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ECommerceWeb.Data;
using ECommerceWeb.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ECommerceWeb.Controllers
{
    /// <summary>
    /// Handles CRUD operations for personal occasions such as birthdays or anniversaries.
    /// All actions require the user to be authenticated.
    /// </summary>
    [Authorize]
    public class OccasionsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OccasionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var occasions = await _db.Occasions.Where(o => o.UserId == userId).ToListAsync();
            return View(occasions);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Occasion occasion)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            occasion.UserId = userId!;
            _db.Occasions.Add(occasion);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}