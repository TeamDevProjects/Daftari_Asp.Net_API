using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class ClientTransactionRepository : Repository<ClientTransaction>, IClientTransactionRepository
	{
		public ClientTransactionRepository(DaftariContext context) : base(context) { }

		public async Task<IEnumerable<ClientsTransactionsView>> GetAllAsync(int userId,int clientId)
		{
			try
			{

				var results = await _context.ClientsTransactionsViews.Where(c=>c.UserId == userId && c.ClientId == clientId ).ToListAsync(); // This retrieves all records in the DbSet
				if (!results.Any()) throw new KeyNotFoundException("no transacrions founded");
				
				return results;
			}
			catch (Exception ex) { throw; }
		}
		


	}
}
