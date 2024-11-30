using Daftari.Dtos.PaymentDates.ClientPaymentDateDtos;
using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Services.IServices
{
	public interface IClientPaymentDateService
	{
		Task<ClientPaymentDate> AddClientPaymentDateAsync(ClientPaymentDateCreateDto clientPaymentDateData);
		Task<ClientPaymentDate> SaveClientPaymentDateAsync(int clientId, decimal totalAmount, int userId);
		Task<ClientPaymentDate> GetClientPaymentDateByIdAsync(int clientPaymentDateId);
		Task<ClientPaymentDate> GetClientPaymentDateByClientIdAsync(int clientId);
		Task<bool> DeleteClientPaymentDateAsync(int clientPaymentDateId);
		Task<bool> DeleteClientPaymentDateByClientIdAsync(int clientId);
		Task<ClientsPaymentDateView> GetPaymentDateViewByClientAsync(int ClientId);
		Task<IEnumerable<ClientsPaymentDateView>> GetAllCloserPaymentsDateAsync(int userId);
		Task<IEnumerable<ClientsPaymentDateView>> GetAllOldPaymentsDateAsync(int userId);
		Task<IEnumerable<ClientsPaymentDateView>> GetAllToDayPaymentsDateAsync(int userId);

	}
}
