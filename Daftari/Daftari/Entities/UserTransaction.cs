using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class UserTransaction
{
    public int UserTransactionId { get; set; }

    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
