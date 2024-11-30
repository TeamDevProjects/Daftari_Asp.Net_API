 
 
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class PaymentMethod
{
    public byte PaymentMethodId { get; set; }

    public string PaymentMethodName { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<PaymentDate> PaymentDates { get; set; } = new List<PaymentDate>();
}
