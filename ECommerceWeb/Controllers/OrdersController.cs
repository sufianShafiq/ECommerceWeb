using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ECommerceWeb.Data;
using ECommerceWeb.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ECommerceWeb.Controllers
{
    /// <summary>
    /// Exposes endpoints for listing orders, viewing order details, repeating an
    /// existing order and leaving feedback. All actions require an authenticated
    /// user.
    /// </summary>
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Lists all orders for the currently logged in user.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var orders = await _db.Orders.Include(o => o.Items).Where(o => o.UserId == userId).ToListAsync();
            return View(orders);
        }

        /// <summary>
        /// Shows the details of a single order.
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        /// <summary>
        /// Creates a new order by copying the items from an existing order. The new
        /// order is immediately saved with a status of Placed.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Repeat(int id)
        {
            var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return NotFound();
            var newOrder = new Order
            {
                UserId = order.UserId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Placed,
                Total = order.Total,
                Items = order.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
            _db.Orders.Add(newOrder);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Stores feedback left by the user for an order.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LeaveFeedback(int id, string feedback)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return NotFound();
            order.Feedback = feedback;
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }
    }
}