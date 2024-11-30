 
 
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class ClientTransaction
{
    public int ClientTransactionId { get; set; }

    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int ClientId { get; set; }

	[JsonIgnore]
	public virtual Client Client { get; set; } = null!;

	[JsonIgnore]
	public virtual Transaction Transaction { get; set; } = null!;

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
