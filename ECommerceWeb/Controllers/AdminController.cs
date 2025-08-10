using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ECommerceWeb.Data;
using System.Threading.Tasks;

namespace ECommerceWeb.Controllers
{
    /// <summary>
    /// Provides a simple admin dashboard. This example restricts access using the
    /// Admin role. In a full implementation additional CRUD pages for users,
    /// orders, addresses, occasions, wallet transactions, favourites and invoices
    /// would be added.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Dashboard()
        {
            var users = await _db.Users.ToListAsync();
            var orders = await _db.Orders.ToListAsync();
            // passing anonymous type to view; strongly typed view models are recommended
            return View(new { Users = users, Orders = orders });
        }
    }
}