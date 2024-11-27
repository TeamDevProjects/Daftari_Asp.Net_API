using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class SupplierPaymentDate
{
    public int SupplierPaymentDateId { get; set; }

    public int UserId { get; set; }

    public int SupplierId { get; set; }

    public int PaymentDateId { get; set; }

	[JsonIgnore]
	public virtual PaymentDate PaymentDate { get; set; } = null!;

	[JsonIgnore]
	public virtual Supplier Supplier { get; set; } = null!;

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
