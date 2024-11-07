using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class ClientPaymentDate
{
    public int ClientPaymentDateId { get; set; }

    public int UserId { get; set; }

    public int ClientId { get; set; }

    public int PaymentDateId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual PaymentDate PaymentDate { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
