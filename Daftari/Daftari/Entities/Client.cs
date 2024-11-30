
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class Client
{
    public int ClientId { get; set; }

    public int PersonId { get; set; }

    public int UserId { get; set; }

    public string? Notes { get; set; }

	[JsonIgnore]
	public virtual ICollection<ClientPaymentDate> ClientPaymentDates { get; set; } = new List<ClientPaymentDate>();

	[JsonIgnore]
	public virtual ICollection<ClientTotalAmount> ClientTotalAmounts { get; set; } = new List<ClientTotalAmount>();

	[JsonIgnore]
	public virtual ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>();

	[JsonIgnore]
	public virtual Person Person { get; set; } = null!;

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
