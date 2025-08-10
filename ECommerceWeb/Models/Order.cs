using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    /// <summary>
    /// Enumeration representing the various states an order can be in. These values
    /// correspond to common e‑commerce order workflows and can be extended as needed.
    /// </summary>
    public enum OrderStatus
    {
        Placed,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    /// <summary>
    /// Represents a customer order consisting of one or more items. Contains metadata
    /// such as the order date, status, total cost and optional customer feedback.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Placed;

        public decimal Total { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public string? Feedback { get; set; }
    }

    /// <summary>
    /// Represents a single line item in an order. Each item has a product name,
    /// quantity and unit price. Additional product details could be included here.
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}