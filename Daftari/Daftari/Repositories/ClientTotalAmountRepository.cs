using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class ClientTotalAmountRepository : Repository<ClientTotalAmount>, IClientTotalAmountRepository
	{
		public ClientTotalAmountRepository(DaftariContext context) : base(context) { }

		public async Task<ClientTotalAmount> GetByClientIdAsync(int clientId)
		{
			return await _context.ClientTotalAmounts.FirstOrDefaultAsync(x => x.ClientId == clientId);
		}
	}
}
