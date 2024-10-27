using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class Sector
{
    public int SectorId { get; set; }

    public int? SectorTypeId { get; set; }

    public string SectorName { get; set; } = null!;

    public virtual SectorType? SectorType { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
