using Daftari.Entities;

namespace Daftari.Services.IServices
{
	public interface ISupplierTotalAmountService
	{
		Task<decimal> AddSupplierTotalAmountAsync(decimal totalAmount, int userId, int supplierId);

		Task<decimal> UpdateSupplierTotalAmountAsync(SupplierTotalAmount existSupplierTotalAmount, decimal Amount, byte TransactionTypeId);


		Task<decimal> SaveSupplierTotalAmountAsync(byte TransactionTypeId, decimal Amount, int supplierId, int userId);

		Task<SupplierTotalAmount> GetSupplierTotalAmountBySupplierId(int supplierId);
	}
}
