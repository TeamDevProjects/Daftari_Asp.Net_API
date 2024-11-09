using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public byte? TransactionTypeId { get; set; }

    public string? Notes { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal Amount { get; set; }

    public byte[]? ImageData { get; set; } = null!;

    public string? ImageType { get; set; }

    public virtual ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>();

    public virtual ICollection<SupplierTransaction> SupplierTransactions { get; set; } = new List<SupplierTransaction>();

    public virtual TransactionType? TransactionType { get; set; }

    public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
}
