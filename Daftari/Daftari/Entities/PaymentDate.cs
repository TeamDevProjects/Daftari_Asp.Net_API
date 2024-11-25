using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class PaymentDate
{
    public int PaymentDateId { get; set; }

    public DateTime DateOfPayment { get; set; }

    public byte PaymentMethodId { get; set; }

    public string? Notes { get; set; }

	[JsonIgnore]
	public virtual ICollection<ClientPaymentDate> ClientPaymentDates { get; set; } = new List<ClientPaymentDate>();

	[JsonIgnore]
	public virtual PaymentMethod PaymentMethod { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<SupplierPaymentDate> SupplierPaymentDates { get; set; } = new List<SupplierPaymentDate>();
}
