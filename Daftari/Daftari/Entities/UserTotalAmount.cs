 
 
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class UserTotalAmount
{
    public int UserTotalAmountId { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime UpdateAt { get; set; }

	[JsonIgnore]
	public virtual User User { get; set; } = null!;
}
