using Daftari.Dtos.PaymentDates.SupplierPaymentDateDtos;
using Daftari.Entities;

namespace Daftari.Services.IServices
{
	public interface ISupplierPaymentDateService
	{
		Task<SupplierPaymentDate> AddSupplierPaymentDateAsync(SupplierPaymentDateCreateDto supplierPaymentDateCreateDto);

		Task<SupplierPaymentDate> GetSupplierPaymentDateAsync(int supplierPaymentDateId);

		Task<SupplierPaymentDate> GetSupplierPaymentDateBySupplierAsync(int supplierId);

		Task<SupplierPaymentDate> SaveSupplierPaymentDateAsync(int supplierId, decimal totalAmount, int userId);
		
		Task<bool> DeleteSupplierPaymentDateAsync(int supplierPaymentDateId);


	}
}
