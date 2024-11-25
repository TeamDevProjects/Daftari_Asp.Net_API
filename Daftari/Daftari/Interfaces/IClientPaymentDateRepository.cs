using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface IClientPaymentDateRepository : IRepository<ClientPaymentDate>
	{
		Task<ClientPaymentDate> GetByClientIdAsync(int clientId);
	}
}
