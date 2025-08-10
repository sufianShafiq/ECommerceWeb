using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    /// <summary>
    /// Represents a date‑based event for a customer such as a birthday or anniversary.
    /// These occasions can be used to remind the user about special dates and trigger
    /// promotional offers.
    /// </summary>
    public class Occasion
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Type { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}