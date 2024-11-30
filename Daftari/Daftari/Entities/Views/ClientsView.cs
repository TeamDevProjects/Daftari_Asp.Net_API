using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Daftari.Entities.Views;

[Keyless]
public partial class ClientsView
{
    public int ClientId { get; set; }
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? Address { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? DateOfPayment { get; set; }

    public string? PaymentMethodName { get; set; }
}
