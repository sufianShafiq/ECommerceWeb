using Microsoft.AspNetCore.Identity;

namespace ECommerceWeb.Models
{
    /// <summary>
    /// Custom application user that extends the built‑in IdentityUser to include additional
    /// profile information relevant to the e‑commerce site such as the user's full name and
    /// wallet balance. More properties can be added here as the business requirements grow.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public decimal WalletBalance { get; set; }
    }
}