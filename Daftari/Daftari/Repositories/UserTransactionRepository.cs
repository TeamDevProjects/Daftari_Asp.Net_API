using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class UserTransactionRepository : Repository<UserTransaction>, IUserTransactionRepository
	{
		public UserTransactionRepository(DaftariContext context) : base(context) { }

		public async Task<IEnumerable<UserTransaction>> GetAllAsync(int userId)
		{
			try
			{

				return await _context.UserTransactions.Where(c => c.UserId == userId).ToListAsync(); // This retrieves all records in the DbSet
			}
			catch (Exception ex) { throw; }
		}

	}
}
