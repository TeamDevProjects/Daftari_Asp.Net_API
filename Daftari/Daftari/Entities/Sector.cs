using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class Sector
{
    public byte SectorId { get; set; }

    public byte? SectorTypeId { get; set; }

    public string SectorName { get; set; } = null!;

	[JsonIgnore]
	public virtual SectorType? SectorType { get; set; }

	[JsonIgnore]
	public virtual ICollection<User> Users { get; set; } = new List<User>();
}
