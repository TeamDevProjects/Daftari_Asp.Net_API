using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class PaymentDate
{
    public int PaymentDateId { get; set; }

    public DateTime DateOfPayment { get; set; } = DateTime.Now;

    public byte PaymentMethodId { get; set; }

    public string? Notes { get; set; }

    public virtual ClientPaymentDate ClientPaymentDate { get; set; } = null!;

	public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual SupplierPaymentDate SupplierPaymentDate { get; set; } = null!;
}
