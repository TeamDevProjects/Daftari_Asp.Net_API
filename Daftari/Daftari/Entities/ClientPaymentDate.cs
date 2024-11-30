using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class ClientPaymentDate
{
    public int ClientPaymentDateId { get; set; }

    public int UserId { get; set; }

    public int ClientId { get; set; }

    public int PaymentDateId { get; set; }

    [JsonIgnore]
    public virtual Client Client { get; set; } = null!;

    [JsonIgnore]
    public virtual PaymentDate PaymentDate { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
