using Daftari.Dtos.People.Client;
using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface IClientRepository : IRepository<Client>
	{
		Task<IEnumerable<ClientsView>> GetAll(int userId);
		Task<IEnumerable<ClientsView>> Search(string temp);
		Task<IEnumerable<ClientsView>> GetOrderedByName(int userId);
		Task<IEnumerable<ClientsView>> GetOrderedByCloserPaymentDates(int userId);
		Task<IEnumerable<ClientsView>> GetOrderedByOlderPaymentDates(int userId);
		Task<IEnumerable<ClientsView>> GetOrderedByLargestTotalAmount(int userId);
		Task<IEnumerable<ClientsView>> GetOrderedBySmallestTotalAmount(int userId);
	}
}