using Microsoft.EntityFrameworkCore;

namespace Daftari.Entities.Views
{
	[Keyless]
	public class SectorsView
	{
		public byte? SectorId { get; set; }
		public string SectorName { get; set; } = null!;
		public string SectorTypeName { get; set; } = null!;
	}
}
