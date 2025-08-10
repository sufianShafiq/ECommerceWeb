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
    /// Provides actions to list and add products to a user's favourites. Removing
    /// favourites could be added later.
    /// </summary>
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FavoritesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var favorites = await _db.FavoriteItems.Where(f => f.UserId == userId).ToListAsync();
            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string productName, string? productId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var fav = new FavoriteItem
            {
                UserId = userId!,
                ProductName = productName,
                ProductId = productId
            };
            _db.FavoriteItems.Add(fav);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}