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

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
