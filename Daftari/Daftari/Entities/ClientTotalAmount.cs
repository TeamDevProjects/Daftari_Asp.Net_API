using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class ClientTotalAmount
{
    public int ClientTotalAmountId { get; set; }

    public int ClientId { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
