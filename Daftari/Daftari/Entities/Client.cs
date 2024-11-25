﻿using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class Client
{
    public int ClientId { get; set; }

    public int PersonId { get; set; }

    public int UserId { get; set; }

    public string Notes { get; set; } = null!;

    public virtual ClientPaymentDate ClientPaymentDate { get; set; } = null!;

    public virtual ClientTotalAmount ClientTotalAmount { get; set; } = null!;

	public virtual ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>();

    public virtual Person Person { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
