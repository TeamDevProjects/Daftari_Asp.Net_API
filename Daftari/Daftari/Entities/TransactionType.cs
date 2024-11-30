 
 
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class TransactionType
{
    public byte TransactionTypeId { get; set; }

    public string TransactionTypeName { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
