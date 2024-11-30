using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class UserTransactionRepository : Repository<UserTransaction>, IUserTransactionRepository
	{
		public UserTransactionRepository(DaftariContext context) : base(context) { }

		public async Task<IEnumerable<UserTransactionsView>> GetAllAsync(int userId)
		{
			try
			{

				return await _context.UserTransactionsViews.Where(c => c.UserId == userId).ToListAsync(); // This retrieves all records in the DbSet
			}
			catch (Exception ex) { throw; }
		}

	}
}
