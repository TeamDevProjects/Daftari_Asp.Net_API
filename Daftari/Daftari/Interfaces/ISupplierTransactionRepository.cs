using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface ISupplierTransactionRepository : IRepository<SupplierTransaction>
	{
		Task<IEnumerable<SuppliersTransactionsView>> GetAllAsync(int userId, int supplierId);

	}
}
