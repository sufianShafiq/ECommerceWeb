using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ECommerceWeb.Data;
using ECommerceWeb.Models;
using System.Threading.Tasks;

namespace ECommerceWeb.Controllers
{
    /// <summary>
    /// Lists invoices and displays details for a single invoice. Downloading or
    /// generating PDF invoices can be implemented in the Download action.
    /// </summary>
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public InvoicesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var invoices = await _db.Invoices.Include(i => i.Order).Where(i => i.Order!.UserId == userId).ToListAsync();
            return View(invoices);
        }

        public async Task<IActionResult> Download(int id)
        {
            var invoice = await _db.Invoices.Include(i => i.Order).FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null)
                return NotFound();
            return View(invoice);
        }
    }
}