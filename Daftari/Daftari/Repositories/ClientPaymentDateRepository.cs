using Daftari.Data;
using Daftari.Entities;
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

		

	}
}
