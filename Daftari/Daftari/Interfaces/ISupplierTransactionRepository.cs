using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface ISupplierTransactionRepository : IRepository<SupplierTransaction>
	{
		Task<IEnumerable<SupplierTransaction>> GetAllAsync(int userId, int supplierId);

	}
}
