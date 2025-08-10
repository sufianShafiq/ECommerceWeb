using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    /// <summary>
    /// Enumerates the types of transactions that can occur in a customer's wallet.
    /// Credit adds funds, Debit removes funds, Bonus grants promotional credits.
    /// </summary>
    public enum TransactionType
    {
        Credit,
        Debit,
        Bonus
    }

    /// <summary>
    /// Represents a wallet transaction. Each entry records the type, amount,
    /// timestamp and an optional description for auditing and display.
    /// </summary>
    public class WalletTransaction
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
        public TransactionType Type { get; set; }
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
    }
}