using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Daftari.Entities.Views;

[Keyless]
public partial class SuppliersPaymentDateView
{
    public int SupplierPaymentDateId { get; set; }

    public int UserId { get; set; }

    public int SupplierId { get; set; }

    public DateTime? DateOfPayment { get; set; }

    public string? Notes { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? PaymentMethodName { get; set; }
}
