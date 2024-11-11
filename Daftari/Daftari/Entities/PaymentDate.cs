using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class PaymentDate
{
    public int PaymentDateId { get; set; }

    public DateTime DateOfPayment { get; set; }

    public decimal TotalAmount { get; set; }

    public byte PaymentMethodId { get; set; } 

    public string? Notes { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

	public virtual ICollection<ClientPaymentDate> ClientPaymentDates { get; set; } = new List<ClientPaymentDate>();

    public virtual ICollection<SupplierPaymentDate> SupplierPaymentDates { get; set; } = new List<SupplierPaymentDate>();
}
