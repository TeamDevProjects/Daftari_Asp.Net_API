using Daftari.Dtos.People.Client;
using Daftari.Entities;
using Daftari.Services.HelperServices;

namespace Daftari.Services.IServices
{
	public interface IClientService
	{
		Task<Client> AddClientAsync(ClientCreateDto clientData, int userId);

		Task<bool> UpdateClientAsync(ClientUpdateDto clientData, int clientId);

		Task<bool> DeleteClientAsync(int clientId);
	}
}
