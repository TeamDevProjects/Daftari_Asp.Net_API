using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class ClientRepository:Repository<Client>, IClientRepository
	{
        public ClientRepository(DaftariContext context):base(context) { }

		// Get All Suppliers that has userId N
		public async Task<IEnumerable<ClientsView>> GetAll(int userId)
		{
			try
			{
				var clients = await _context.ClientsViews.Where((c)=>c.UserId == userId).ToListAsync();

				if (clients.Any()) return clients;

				return null;
			}
			catch (Exception) { return null; }
		}

		// Search for Supplier Name [ start, middle, end ]
		public async Task<IEnumerable<ClientsView>> SearchByName(string temp)
		{
			try
			{
				var clients = await _context.ClientsViews.Where((u) => u.Name.Contains(temp)).ToListAsync();

				if (clients.Any()) return clients;

				return null;
			}
			catch (Exception) { return null; }
		}

		// Get All Ordered by [ A : Z ]
		public async Task<IEnumerable<ClientsView>> GetAllOrderedByName(int userId)
		{
			try
			{
				var clients = await _context.ClientsViews.Where((c) => c.UserId == userId).OrderBy((c)=>c.Name).ToListAsync();

				if (clients.Any()) return clients;

				return null;
			}
			catch (Exception) { return null; }
		}



	}
}
