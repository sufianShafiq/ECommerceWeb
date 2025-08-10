using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommerceWeb.Models;

namespace ECommerceWeb.Data
{
    /// <summary>
    /// Application database context configured for Identity and the domain entities used in the
    /// e‑commerce customer portal. The context exposes DbSet properties for each entity so
    /// Entity Framework Core can create corresponding tables and relationships.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Occasion> Occasions => Set<Occasion>();
        public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();
        public DbSet<FavoriteItem> FavoriteItems => Set<FavoriteItem>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
    }
}