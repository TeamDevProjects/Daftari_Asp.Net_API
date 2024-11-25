using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface IClientTotalAmountRepository : IRepository<ClientTotalAmount>
	{
		Task<ClientTotalAmount> GetByClientIdAsync(int clientId);
	}
}
