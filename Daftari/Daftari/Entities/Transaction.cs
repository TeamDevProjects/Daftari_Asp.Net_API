 
 
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
	public virtual ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>();

	[JsonIgnore]
	public virtual ICollection<SupplierTransaction> SupplierTransactions { get; set; } = new List<SupplierTransaction>();

	[JsonIgnore]
	public virtual TransactionType? TransactionType { get; set; }
	
	[JsonIgnore]
	public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
}
