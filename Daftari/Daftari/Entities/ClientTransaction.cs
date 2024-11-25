using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class ClientTransaction
{
    public int ClientTransactionId { get; set; }

    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
