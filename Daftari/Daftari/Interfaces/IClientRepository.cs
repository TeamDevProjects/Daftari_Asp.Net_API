using Daftari.Dtos.People.Client;
using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface IClientRepository : IRepository<Client>
	{
		Task<IEnumerable<ClientsView>> GetAll(int userId);
		Task<IEnumerable<ClientsView>> SearchByName(string temp);
		Task<IEnumerable<ClientsView>> GetAllOrderedByName(int userId);

	}
}
