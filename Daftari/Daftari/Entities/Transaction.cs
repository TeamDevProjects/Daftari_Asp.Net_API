using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public byte TransactionTypeId { get; set; }

    public string? Notes { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal Amount { get; set; }

    public byte[]? ImageData { get; set; }

    public string? ImageType { get; set; }

	[JsonIgnore]
	public virtual ClientTransaction ClientTransaction { get; set; } = null!;

	[JsonIgnore]
	public virtual SupplierTransaction SupplierTransaction { get; set; } = null!;

    public virtual TransactionType? TransactionType { get; set; }

	[JsonIgnore]
	public virtual UserTransaction UserTransaction { get; set; } = null!;
}
