using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class ClientTotalAmount
{
    public int ClientTotalAmountId { get; set; }

    public int ClientId { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime UpdateAt { get; set; }

	[JsonIgnore]
	public virtual Client Client { get; set; } = null!;

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
