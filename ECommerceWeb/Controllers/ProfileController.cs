using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ECommerceWeb.Models;
using System.Threading.Tasks;

namespace ECommerceWeb.Controllers
{
    /// <summary>
    /// Allows authenticated users to view and update their personal details. Edits
    /// are limited to simple fields like full name, email and phone number.
    /// </summary>
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Displays the currently authenticated user's profile.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        /// <summary>
        /// Accepts posted profile changes and updates the user record. On success
        /// the user is redirected back to the profile page.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ApplicationUser model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}