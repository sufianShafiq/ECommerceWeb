using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    /// <summary>
    /// Represents a postal or delivery address for a user. Addresses may be collected from
    /// orders or added manually. Latitude and longitude fields can be used for mapping
    /// integrations. Additional optional lines allow users to include apartment or suite
    /// details.
    /// </summary>
    public class Address
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }

        [Required]
        public string Line1 { get; set; } = string.Empty;

        public string? Line2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}