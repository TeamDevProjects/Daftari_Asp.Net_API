using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class PaymentMethod
{
    public byte PaymentMethodId { get; set; }

    public string PaymentMethodName { get; set; } = null!;
}
