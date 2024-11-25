using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface IClientTransactionRepository : IRepository<ClientTransaction>
	{
		Task<IEnumerable<ClientTransaction>> GetAllAsync(int userId, int clientId);
	}
}
