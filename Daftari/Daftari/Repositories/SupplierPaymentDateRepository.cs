using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class SupplierPaymentDateRepository : Repository<SupplierPaymentDate>, ISupplierPaymentDateRepository
	{
		public SupplierPaymentDateRepository(DaftariContext context) : base(context) { }

		public async Task<SupplierPaymentDate> GetBySuppliertIdAsync(int supplierId)
		{
			return await _context.SupplierPaymentDates.FirstOrDefaultAsync(x => x.SupplierId == supplierId);
		}
	}
}
