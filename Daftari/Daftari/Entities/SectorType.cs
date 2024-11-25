using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class SectorType
{
    public byte SectorTypeId { get; set; }

    public string SectorTypeName { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<Sector> Sectors { get; set; } = new List<Sector>();
}
