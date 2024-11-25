using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class SectorsView
{
    public byte SectorId { get; set; }

    public string SectorName { get; set; } = null!;

    public string SectorTypeName { get; set; } = null!;
}
