using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface ISupplierTotalAmountRepository :IRepository<SupplierTotalAmount>
	{
		Task<SupplierTotalAmount> GetBySupplierIdAsync(int supplierId);
	}
}
