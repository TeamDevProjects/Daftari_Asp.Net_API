using Daftari.Dtos.People.Supplier;
using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface ISupplierRepository : IRepository<Supplier>
	{
		Task<IEnumerable<SuppliersView>> GetAll(int userId);
		Task<IEnumerable<SuppliersView>> Search(string temp);
		Task<IEnumerable<SuppliersView>> GetOrderedByName(int userId);
		Task<IEnumerable<SuppliersView>> GetOrderedByCloserPaymentDates(int userId);
		Task<IEnumerable<SuppliersView>> GetOrderedByOlderPaymentDates(int userId);
		Task<IEnumerable<SuppliersView>> GetOrderedByLargestTotalAmount(int userId);
		Task<IEnumerable<SuppliersView>> GetOrderedBySmallestTotalAmount(int userId);
	}
}
