using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface ISupplierPaymentDateRepository :IRepository<SupplierPaymentDate>
	{
		Task<SupplierPaymentDate> GetBySuppliertIdAsync(int supplierId);
		Task<SuppliersPaymentDateView> GetPaymentDateViewAsync(int supplierId);
		Task<IEnumerable<SuppliersPaymentDateView>> GetAllToDayPaymentsDateViewAsync(int userId);
		Task<IEnumerable<SuppliersPaymentDateView>> GetAllCloserPaymentsDateViewAsync(int userId);
		Task<IEnumerable<SuppliersPaymentDateView>> GetAllOldPaymentsDateViewAsync(int userId);

	}
}
