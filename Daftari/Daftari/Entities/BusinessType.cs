using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class BusinessType
{
    public byte BusinessTypeId { get; set; }

    public string BusinessTypeName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
