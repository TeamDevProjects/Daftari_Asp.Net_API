 
 
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class SupplierTotalAmount
{
    public int SupplierTotalAmountId { get; set; }

    public int SupplierId { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime UpdateAt { get; set; }

	[JsonIgnore]
	public virtual Supplier Supplier { get; set; } = null!;

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
