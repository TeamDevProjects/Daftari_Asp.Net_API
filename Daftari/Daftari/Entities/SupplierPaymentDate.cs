using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class SupplierPaymentDate
{
    public int SupplierPaymentDateId { get; set; }

    public int UserId { get; set; }

    public int SupplierId { get; set; }

    public int PaymentDateId { get; set; }

    public virtual PaymentDate PaymentDate { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
