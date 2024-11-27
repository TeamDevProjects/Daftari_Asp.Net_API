using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class SupplierTransaction
{
    public int SupplierTransactionId { get; set; }

    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int SupplierId { get; set; }

	[JsonIgnore]
	public virtual Supplier Supplier { get; set; } = null!;

	[JsonIgnore]
	public virtual Transaction Transaction { get; set; } = null!;

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
