using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class SupplierTransaction
{
    public int SupplierTransactionId { get; set; }

    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int SupplierId { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
