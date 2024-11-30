using Daftari.Dtos.People.Supplier;
using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface ISupplierRepository : IRepository<Supplier>
	{
		Task<IEnumerable<SuppliersView>> GetAll(int userId);
		Task<IEnumerable<SuppliersView>> SearchByName(string temp);
		Task<IEnumerable<SuppliersView>> GetAllOrderedByName(int userId);
	}
}
