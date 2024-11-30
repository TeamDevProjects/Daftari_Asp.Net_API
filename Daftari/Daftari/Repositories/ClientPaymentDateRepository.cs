using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class ClientPaymentDateRepository : Repository<ClientPaymentDate>, IClientPaymentDateRepository
	{
		public ClientPaymentDateRepository(DaftariContext context) : base(context) { }

		public async Task<ClientPaymentDate> GetByClientIdAsync(int clientId)
		{
			return await _context.ClientPaymentDates.FirstOrDefaultAsync(x => x.ClientId == clientId);
		}

		// -> ClientPaymentDatesView
		public async Task<ClientsPaymentDateView> GetPaymentDateViewAsync(int ClientId)
		{
			var paymentDate = await _context.ClientsPaymentDateViews.FirstOrDefaultAsync(x => x.ClientId == ClientId);
			return paymentDate!;
		}

		// Get ToDay Client PaymentDates
		public async Task<IEnumerable<ClientsPaymentDateView>> GetAllToDayPaymentsDateViewAsync(int userId)
		{
			return await _context.ClientsPaymentDateViews
				.Where(x => x.UserId == userId && EF.Functions.DateDiffDay(x.DateOfPayment, DateTime.Today) == 0)
				.ToListAsync();
		}

		// Get Closer Client PaymentDates
		public async Task<IEnumerable<ClientsPaymentDateView>> GetAllCloserPaymentsDateViewAsync(int userId)
		{
			var today = DateTime.Today;
			return await _context.ClientsPaymentDateViews
				.Where(x => x.UserId == userId && x.DateOfPayment > today)
				.OrderBy(x => x.DateOfPayment) // Closest dates will appear first
				.ToListAsync();
		}

		// Get Old Client PaymentDates
		public async Task<IEnumerable<ClientsPaymentDateView>> GetAllOldPaymentsDateViewAsync(int userId)
		{
			var today = DateTime.Today;
			return await _context.ClientsPaymentDateViews
				.Where(x => x.UserId == userId && x.DateOfPayment < today)
				.OrderByDescending(x => x.DateOfPayment) // Most recent old dates will appear first
				.ToListAsync();
		}



	}
}
