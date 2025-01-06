using Daftari.Dtos.People.Client;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Services.HelperServices;

namespace Daftari.Services.IServices
{
	public interface IClientService
	{
		Task<ClientsView> AddClientAsync(ClientCreateDto clientData, int userId);

		Task<bool> UpdateClientAsync(ClientUpdateDto clientData, int clientId);

		Task<bool> DeleteClientAsync(int clientId);
		Task<IEnumerable<ClientsView>> GetAllClients(int userId);
		Task<IEnumerable<ClientsView>> SearchForClients(string temp);
		Task<IEnumerable<ClientsView>> GetAllOrderedByName(int userId);
		Task<IEnumerable<ClientsView>> GetAllOrderedByCloserPaymentDates(int userId);
		Task<IEnumerable<ClientsView>> GetAllOrderedByOlderPaymentDates(int userId);
		Task<IEnumerable<ClientsView>> GetAllOrderedByLargestTotalAmount(int userId);
		Task<IEnumerable<ClientsView>> GetAllOrderedBySmallestTotalAmount(int userId);

	}
}
