﻿using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class SectorType
{
    public byte SectorTypeId { get; set; }

    public string SectorTypeName { get; set; } = null!;

    public virtual ICollection<Sector> Sectors { get; set; } = new List<Sector>();
}
