using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface IClientTransactionRepository : IRepository<ClientTransaction>
	{
		Task<IEnumerable<ClientsTransactionsView>> GetAllAsync(int userId, int clientId);
	}
}
