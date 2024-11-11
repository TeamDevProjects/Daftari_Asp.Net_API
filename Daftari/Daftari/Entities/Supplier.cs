using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public int PersonId { get; set; }

    public int UserId { get; set; }

    public string Notes { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<SupplierPaymentDate> SupplierPaymentDates { get; set; } = new List<SupplierPaymentDate>();

    public virtual ICollection<SupplierTransaction> SupplierTransactions { get; set; } = new List<SupplierTransaction>();
   
    public virtual ICollection<SupplierTotalAmount> SupplierTotalAmounts { get; set; } = new List<SupplierTotalAmount>();

    public virtual User User { get; set; } = null!;
}
