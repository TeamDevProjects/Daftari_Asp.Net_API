﻿using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class TransactionType
{
    public byte TransactionTypeId { get; set; }

    public string TransactionTypeName { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}