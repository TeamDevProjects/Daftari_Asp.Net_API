using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Daftari.Entities.Views;

[Keyless]
public partial class UserTransactionsView
{
    public int UserTransactionId { get; set; }

    public int UserId { get; set; }

    public string? TransactionTypeName { get; set; }

    public string? Notes { get; set; }

    public DateTime? TransactionDate { get; set; }

    public decimal? Amount { get; set; }

    public byte[]? ImageData { get; set; }

    public string? ImageType { get; set; }
}
