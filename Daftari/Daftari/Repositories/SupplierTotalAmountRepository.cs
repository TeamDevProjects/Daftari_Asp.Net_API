using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class SupplierTotalAmountRepository : Repository<SupplierTotalAmount>, ISupplierTotalAmountRepository
	{
		public SupplierTotalAmountRepository(DaftariContext context) : base(context) { }

		public async Task<SupplierTotalAmount> GetBySupplierIdAsync(int supplierId)
		{
			return await _context.SupplierTotalAmounts.FirstOrDefaultAsync(x => x.SupplierId == supplierId);
		}
	}
}
