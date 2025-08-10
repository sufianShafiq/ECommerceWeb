namespace ECommerceWeb.Models
{
    /// <summary>
    /// Represents a product that a user has saved to their favourites list. This
    /// lightweight entity stores the product name and an optional unique identifier.
    /// </summary>
    public class FavoriteItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductId { get; set; }
    }
}