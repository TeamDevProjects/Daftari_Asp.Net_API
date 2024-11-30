using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface IClientPaymentDateRepository : IRepository<ClientPaymentDate>
	{
		Task<ClientPaymentDate> GetByClientIdAsync(int clientId);
		Task<ClientsPaymentDateView> GetPaymentDateViewAsync(int ClientId);
		Task<IEnumerable<ClientsPaymentDateView>> GetAllToDayPaymentsDateViewAsync(int userId);
		Task<IEnumerable<ClientsPaymentDateView>> GetAllCloserPaymentsDateViewAsync(int userId);
		Task<IEnumerable<ClientsPaymentDateView>> GetAllOldPaymentsDateViewAsync(int userId);

	}
}
