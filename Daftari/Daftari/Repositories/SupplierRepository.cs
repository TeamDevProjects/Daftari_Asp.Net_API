using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class SupplierRepository : Repository<Supplier>, ISupplierRepository
	{
		public SupplierRepository(DaftariContext context) : base(context) { }

		// Get All Suppliers that has userId N
		public async Task<IEnumerable<SuppliersView>> GetAll(int userId)
		{
			try
			{
				var suppliers = await _context.SuppliersViews.Where((c) => c.UserId == userId).ToListAsync();

				if (suppliers.Any()) return suppliers;

				return null;
			}
			catch (Exception) { return null; }
		}

		// Search for Supplier Name [ start, middle, end ]
		public async Task<IEnumerable<SuppliersView>> SearchByName(string temp)
		{
			try
			{
				var suppliers = await _context.SuppliersViews.Where((u) => u.Name.Contains(temp)).ToListAsync();

				if (suppliers.Any()) return suppliers;

				return null;
			}
			catch (Exception) { return null; }
		}

		// Get All Ordered by [ A : Z ]
		public async Task<IEnumerable<SuppliersView>> GetAllOrderedByName(int userId)
		{
			try
			{
				var suppliers = await _context.SuppliersViews.Where((c) => c.UserId == userId).OrderBy((c) => c.Name).ToListAsync();

				if (suppliers.Any()) return suppliers;

				return null;
			}
			catch (Exception) { return null; }
		}
		


	}
}
