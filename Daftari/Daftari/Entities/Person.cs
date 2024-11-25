using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class Person
{
    public int PersonId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

	public virtual User User { get; set; } = null!;
}
