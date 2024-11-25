using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class SupplierTotalAmount
{
    public int SupplierTotalAmountId { get; set; }

    public int SupplierId { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
