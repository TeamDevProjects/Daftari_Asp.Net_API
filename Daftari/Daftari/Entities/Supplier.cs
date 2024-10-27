using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public int PersonId { get; set; }

    public int UserId { get; set; }

    public string Notes { get; set; } = null!;

    public DateOnly DateOfPayment { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
