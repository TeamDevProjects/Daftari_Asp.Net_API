using Daftari.Dtos.PaymentDates.SupplierPaymentDateDtos;
using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Services.IServices
{
	public interface ISupplierPaymentDateService
	{ 
		Task<SupplierPaymentDate> AddSupplierPaymentDateAsync(SupplierPaymentDateCreateDto supplierPaymentDateCreateDto);
		Task<SupplierPaymentDate> GetSupplierPaymentDateAsync(int supplierPaymentDateId);
		Task<SupplierPaymentDate> GetSupplierPaymentDateBySupplierAsync(int supplierId);
		Task<SupplierPaymentDate> SaveSupplierPaymentDateAsync(int supplierId, decimal totalAmount, int userId);
		Task<bool> DeleteSupplierPaymentDateAsync(int supplierPaymentDateId);
		Task<SuppliersPaymentDateView> GetPaymentDateViewBySupplierAsync(int supplierId);
		Task<IEnumerable<SuppliersPaymentDateView>> GetAllCloserPaymentsDateAsync(int userId);
		Task<IEnumerable<SuppliersPaymentDateView>> GetAllOldPaymentsDateAsync(int userId);
		Task<IEnumerable<SuppliersPaymentDateView>> GetAllToDayPaymentsDateAsync(int userId);

	}
}
