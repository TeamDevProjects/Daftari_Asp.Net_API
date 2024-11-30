 
 
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public int PersonId { get; set; }

    public int UserId { get; set; }

    public string? Notes { get; set; }

	[JsonIgnore]
	public virtual Person Person { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<SupplierPaymentDate> SupplierPaymentDates { get; set; } = new List<SupplierPaymentDate>();

	[JsonIgnore]
	public virtual ICollection<SupplierTotalAmount> SupplierTotalAmounts { get; set; } = new List<SupplierTotalAmount>();

	[JsonIgnore]
	public virtual ICollection<SupplierTransaction> SupplierTransactions { get; set; } = new List<SupplierTransaction>();

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
