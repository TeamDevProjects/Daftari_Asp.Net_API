using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface ISupplierPaymentDateRepository :IRepository<SupplierPaymentDate>
	{
		Task<SupplierPaymentDate> GetBySuppliertIdAsync(int supplierId);
	}
}
