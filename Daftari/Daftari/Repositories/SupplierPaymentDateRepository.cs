using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
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

		// -> SupplierPaymentDatesView
		public async Task<SuppliersPaymentDateView> GetPaymentDateViewAsync(int supplierId)
		{
			return await _context.SuppliersPaymentDateViews.FirstOrDefaultAsync(x => x.SupplierId == supplierId);
		}

		// Get ToDay Supplier PaymentDates
		public async Task<IEnumerable<SuppliersPaymentDateView>> GetAllToDayPaymentsDateViewAsync(int userId)
		{
			return await _context.SuppliersPaymentDateViews
				.Where(x => x.UserId == userId && EF.Functions.DateDiffDay(x.DateOfPayment, DateTime.Today) == 0)
				.ToListAsync();
		}

		// Get Closer Supplier PaymentDates
		public async Task<IEnumerable<SuppliersPaymentDateView>> GetAllCloserPaymentsDateViewAsync(int userId)
		{
			var today = DateTime.Today;
			return await _context.SuppliersPaymentDateViews
				.Where(x => x.UserId == userId && x.DateOfPayment > today)
				.OrderBy(x => x.DateOfPayment) // Closest dates will appear first
				.ToListAsync();
		}

		// Get Old Supplier PaymentDates
		public async Task<IEnumerable<SuppliersPaymentDateView>> GetAllOldPaymentsDateViewAsync(int userId)
		{
			var today = DateTime.Today;
			return await _context.SuppliersPaymentDateViews
				.Where(x => x.UserId == userId && x.DateOfPayment < today)
				.OrderByDescending(x => x.DateOfPayment) // Most recent old dates will appear first
				.ToListAsync();
		}
	}
}
