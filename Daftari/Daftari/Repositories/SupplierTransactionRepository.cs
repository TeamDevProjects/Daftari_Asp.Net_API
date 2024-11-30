using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class SupplierTransactionRepository : Repository<SupplierTransaction>, ISupplierTransactionRepository
	{
		public SupplierTransactionRepository(DaftariContext context) : base(context) { }

		public async Task<IEnumerable<SuppliersTransactionsView>> GetAllAsync(int userId, int supplierId)
		{
			try
			{

				return await _context.SuppliersTransactionsViews.Where(c => c.UserId == userId && c.SupplierId == supplierId).ToListAsync(); // This retrieves all records in the DbSet
			}
			catch (Exception ex) { throw; }
		}

	}
}
