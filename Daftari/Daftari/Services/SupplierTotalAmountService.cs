using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Services.IServices;

namespace Daftari.Services
{
    public class SupplierTotalAmountService: ISupplierTotalAmountService
	{
        private readonly ISupplierTotalAmountRepository _supplierTotalAmountRepository;
        public SupplierTotalAmountService(ISupplierTotalAmountRepository supplierTotalAmountRepository)
        {
            _supplierTotalAmountRepository = supplierTotalAmountRepository;
        }


        public async Task<decimal> AddSupplierTotalAmountAsync(decimal totalAmount, int userId, int supplierId)
        {
            var newSupplierTotalAmount = new SupplierTotalAmount
            {
                UpdateAt = DateTime.UtcNow,
                UserId = userId,
                SupplierId = supplierId,
                TotalAmount = totalAmount
            };

            var suppliertTotalAmount = await _supplierTotalAmountRepository.AddAsync(newSupplierTotalAmount);

            if (suppliertTotalAmount == null) throw new InvalidOperationException("Unable to add suppliertTotalAmount");
            return suppliertTotalAmount.TotalAmount;
        }

        public async Task<decimal> UpdateSupplierTotalAmountAsync(SupplierTotalAmount existSupplierTotalAmount, decimal Amount, byte TransactionTypeId)
        {

            decimal totalAmount = Amount;

            if (TransactionTypeId == 1)
            {
                totalAmount = existSupplierTotalAmount.TotalAmount - Amount;

                existSupplierTotalAmount.TotalAmount = totalAmount;
            }
            else if (TransactionTypeId == 2)
            {
                totalAmount = existSupplierTotalAmount.TotalAmount + Amount;

                existSupplierTotalAmount.TotalAmount = totalAmount;
            }

            existSupplierTotalAmount.UpdateAt = DateTime.UtcNow; // Update timestamp

            var isUpdated = await _supplierTotalAmountRepository.UpdateAsync(existSupplierTotalAmount);

            if (!isUpdated) throw new InvalidOperationException("Unable to updated supplier total amount");

            return totalAmount;
        }


        public async Task<decimal> SaveSupplierTotalAmountAsync(byte TransactionTypeId, decimal Amount, int supplierId, int userId)
        {
            decimal totalAmount = Amount;

            var existSupplierTotalAmount = await _supplierTotalAmountRepository.GetBySupplierIdAsync(supplierId);

            if (existSupplierTotalAmount == null)
            {
                var suppliertTotalAmountAdded = await AddSupplierTotalAmountAsync(totalAmount, userId, supplierId);
                totalAmount = suppliertTotalAmountAdded;

            }
            else
            {
                totalAmount = await UpdateSupplierTotalAmountAsync(existSupplierTotalAmount, Amount, TransactionTypeId);
            }


            return totalAmount;
        }

        public async Task<SupplierTotalAmount> GetSupplierTotalAmountBySupplierId(int supplierId)
        {
            var supplierTotalAmount = await _supplierTotalAmountRepository.GetBySupplierIdAsync(supplierId);

            if (supplierTotalAmount == null) throw new KeyNotFoundException("Unable to find supplierId");

            return supplierTotalAmount;
        }

    }
}
