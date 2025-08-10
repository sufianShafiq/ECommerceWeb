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
    /// Manages wallet transactions and displays the user's balance. Users can add
    /// credit and view their transaction history.
    /// </summary>
    [Authorize]
    public class WalletController : Controller
    {
        private readonly ApplicationDbContext _db;

        public WalletController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var transactions = await _db.WalletTransactions.Where(w => w.UserId == userId).ToListAsync();
            var balance = transactions.Sum(t => t.Type == TransactionType.Debit ? -t.Amount : t.Amount);
            ViewBag.Balance = balance;
            return View(transactions);
        }

        public IActionResult AddCredit() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCredit(decimal amount)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var transaction = new WalletTransaction
            {
                UserId = userId!,
                Amount = amount,
                Type = TransactionType.Credit,
                Description = "Add credit"
            };
            _db.WalletTransactions.Add(transaction);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}