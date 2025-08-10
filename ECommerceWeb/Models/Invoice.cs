using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    /// <summary>
    /// Represents a tax‑compliant invoice generated for an order. The PDF URL
    /// property can be used to link to a stored document when implemented.
    /// </summary>
    public class Invoice
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string? TaxNumber { get; set; }
        public string? PdfUrl { get; set; }
    }
}