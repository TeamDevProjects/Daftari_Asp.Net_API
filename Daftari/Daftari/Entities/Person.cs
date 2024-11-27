using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class Person
{
    public int PersonId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Address { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

	[JsonIgnore]
	public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

	[JsonIgnore]
	public virtual ICollection<User> Users { get; set; } = new List<User>();
}
