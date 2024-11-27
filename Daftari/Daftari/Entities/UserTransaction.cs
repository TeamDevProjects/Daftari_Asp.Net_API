using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class UserTransaction
{
    public int UserTransactionId { get; set; }

    public int TransactionId { get; set; }

    public int UserId { get; set; }

    [JsonIgnore]
    public virtual Transaction Transaction { get; set; } = null!;

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
