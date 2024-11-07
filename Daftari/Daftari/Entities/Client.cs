using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class Client
{
    public int ClientId { get; set; }

    public int PersonId { get; set; }

    public int UserId { get; set; }

    public string Notes { get; set; } = null!;

    public virtual ICollection<ClientPaymentDate> ClientPaymentDates { get; set; } = new List<ClientPaymentDate>();

    public virtual ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>();

    public virtual Person Person { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
