using Daftari.Dtos.Transactions.SupplierTransactionDtos;
using Daftari.Entities;
using Daftari.Services.Images;

namespace Daftari.Services.IServices
{
    public interface ISupplierTransactionService
	{
		Task<SupplierTransaction> AddSupplierTransactionAsync(SupplierTransactionCreateDto SupplierTransactionData, int userId);
		
		Task<bool> UpdateSupplierTransactionAsync(SupplierTransactionUpdateDto SupplierTransactionData);
		
		Task<bool> DeleteAsync(int supplierTransactionId, int userId);

		Task<SupplierTransaction> GetAsync(int supplierTransactionId);

		Task<IEnumerable<SupplierTransaction>> GetAllAsync(int userId, int supplierId);
	}
}
