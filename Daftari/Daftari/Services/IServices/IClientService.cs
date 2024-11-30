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
		Task<IEnumerable<ClientsView>> GetAllClientsOrderedByName(int userId);
		Task<IEnumerable<ClientsView>> SearchForClientsByName(string temp);

	}
}
