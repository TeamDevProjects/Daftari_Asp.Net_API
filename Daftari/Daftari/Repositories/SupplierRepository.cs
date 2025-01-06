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
				var Suppliers = await _context.SuppliersViews.Where((s) => s.UserId == userId).ToListAsync();

				return Suppliers;
			}
			catch (Exception) { return null; }
		}

		// Search for Supplier Name [ start, middle, end ]
		public async Task<IEnumerable<SuppliersView>> Search(string temp)
		{
			try
			{
				var Suppliers = await _context.SuppliersViews.Where((u) => u.Name.Contains(temp) || u.Phone.Contains(temp)).ToListAsync();

				return Suppliers;
			}
			catch (Exception) { return null; }
		}

		// Get All Ordered by [ A : Z ]
		public async Task<IEnumerable<SuppliersView>> GetOrderedByName(int userId)
		{
			try
			{
				var Suppliers = await _context.SuppliersViews.Where((s) => s.UserId == userId).OrderBy((s) => s.Name).ToListAsync();

				return Suppliers;
			}
			catch (Exception) { return null; }
		}

		// Get All Ordered by PaymentDates ASC & DSC
		public async Task<IEnumerable<SuppliersView>> GetOrderedByCloserPaymentDates(int userId)
		{
			try
			{
				var Suppliers = await _context.SuppliersViews.Where((s) => s.UserId == userId).OrderBy((s) => s.DateOfPayment).ToListAsync();

				return Suppliers;
			}
			catch (Exception) { return null; }
		}

		public async Task<IEnumerable<SuppliersView>> GetOrderedByOlderPaymentDates(int userId)
		{
			try
			{
				var Suppliers = await _context.SuppliersViews.Where((s) => s.UserId == userId).OrderByDescending((s) => s.DateOfPayment).ToListAsync();

				return Suppliers;
			}
			catch (Exception) { return null; }
		}

		// Get All Ordered by TotalAmount ASC & DSC
		public async Task<IEnumerable<SuppliersView>> GetOrderedByLargestTotalAmount(int userId)
		{
			try
			{
				var Suppliers = await _context.SuppliersViews.Where((s) => s.UserId == userId).OrderByDescending((s) => s.TotalAmount).ToListAsync();

				return Suppliers;
			}
			catch (Exception) { return null; }
		}
		
		public async Task<IEnumerable<SuppliersView>> GetOrderedBySmallestTotalAmount(int userId)
		{
			try
			{
				var Suppliers = await _context.SuppliersViews.Where((s) => s.UserId == userId).OrderBy((s) => s.TotalAmount).ToListAsync();

				return Suppliers;
			}
			catch (Exception) { return null; }
		}
		


	}
}
