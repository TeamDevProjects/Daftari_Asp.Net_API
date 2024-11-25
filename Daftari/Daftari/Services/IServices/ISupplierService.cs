using Daftari.Dtos.People.Supplier;
using Daftari.Entities;
using Daftari.Services.HelperServices;

namespace Daftari.Services.IServices
{
	public interface ISupplierService
	{
		Task<Supplier> AddSupplierAsync(SupplierCreateDto SupplierData, int userId);
		Task<bool> UpdateSupplierAsync(SupplierUpdateDto SupplierData, int supplierId);
		Task<bool> DeleteSupplierAsync(int SupplierId);

	}
}
