using Daftari.Dtos.People.Supplier;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Services.HelperServices;

namespace Daftari.Services.IServices
{
	public interface ISupplierService
	{
		Task<SuppliersView> AddSupplierAsync(SupplierCreateDto SupplierData, int userId);
		Task<bool> UpdateSupplierAsync(SupplierUpdateDto SupplierData, int supplierId);
		Task<bool> DeleteSupplierAsync(int SupplierId);

		Task<IEnumerable<SuppliersView>> GetAllSuppliers(int userId);
		Task<IEnumerable<SuppliersView>> SearchForSuppliers(string temp);
		Task<IEnumerable<SuppliersView>> GetAllOrderedByName(int userId);
		Task<IEnumerable<SuppliersView>> GetAllOrderedByCloserPaymentDates(int userId);
		Task<IEnumerable<SuppliersView>> GetAllOrderedByOlderPaymentDates(int userId);
		Task<IEnumerable<SuppliersView>> GetAllOrderedByLargestTotalAmount(int userId);
		Task<IEnumerable<SuppliersView>> GetAllOrderedBySmallestTotalAmount(int userId);


	}
}
