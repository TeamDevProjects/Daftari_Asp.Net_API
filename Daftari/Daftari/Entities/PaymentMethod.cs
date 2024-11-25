using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class PaymentMethod
{
    public byte PaymentMethodId { get; set; }

    public string PaymentMethodName { get; set; } = null!;

    public virtual ICollection<PaymentDate> PaymentDates { get; set; } = new List<PaymentDate>();
}
