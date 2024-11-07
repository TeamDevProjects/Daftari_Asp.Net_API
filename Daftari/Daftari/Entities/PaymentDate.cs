using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class PaymentDate
{
    public int PaymentDateId { get; set; }

    public DateOnly PaymentDate1 { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual ICollection<ClientPaymentDate> ClientPaymentDates { get; set; } = new List<ClientPaymentDate>();

    public virtual ICollection<SupplierPaymentDate> SupplierPaymentDates { get; set; } = new List<SupplierPaymentDate>();
}
