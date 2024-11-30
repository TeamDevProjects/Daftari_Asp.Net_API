using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Daftari.Entities.Views;

[Keyless]
public partial class ClientsPaymentDateView
{
    public int ClientPaymentDateId { get; set; }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public DateTime? DateOfPayment { get; set; }

    public string? PaymentMethodName { get; set; }

    public string? Notes { get; set; }

    public int ClientId { get; set; }

    public string? Phone { get; set; }
}
