using Daftari.Data;
using Daftari.Dtos.Transactions.ClientTransactionDto;
using Daftari.Dtos.Transactions.SupplierTransactionDtos;
using Daftari.Entities;
using Daftari.Enums;
using Daftari.Interfaces;
using Daftari.Repositories;
using Daftari.Services.Images;
using Daftari.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services
{
    public class SupplierTransactionService: ISupplierTransactionService
	{

        private readonly ITransactionRepository _transactionRepository;
        private readonly ISupplierTransactionRepository _supplierTransactionRepository;

        private readonly ISupplierPaymentDateService _supplierPaymentDateService;
        private readonly ISupplierTotalAmountService _supplierTotalAmountService;
        public SupplierTransactionService(DaftariContext context,
            ISupplierPaymentDateService supplierPaymentDateService, ISupplierTotalAmountService supplierTotalAmountService
            , ITransactionRepository transactionRepository, ISupplierTransactionRepository supplierTransactionRepository)
        {
            _supplierPaymentDateService = supplierPaymentDateService;
            _supplierTotalAmountService = supplierTotalAmountService;
            _transactionRepository = transactionRepository;
            _supplierTransactionRepository = supplierTransactionRepository;
        }

        public async Task<SupplierTransaction> AddSupplierTransactionAsync(SupplierTransactionCreateDto SupplierTransactionData, int userId)
        {
            // Handel Uploading Image
            var ImageObj = await ImageHelper.HandelImageServices(SupplierTransactionData.FormImage!);

            SupplierTransactionData.ImageData = ImageObj.ImageData;
            SupplierTransactionData.ImageType = ImageObj.ImageType;

            // create Base Transaction
            var newTransaction = new Transaction
            {
                TransactionTypeId = SupplierTransactionData.TransactionTypeId,
                Notes = SupplierTransactionData.Notes,
                TransactionDate = DateTime.Now,
                Amount = SupplierTransactionData.Amount,
                ImageData = SupplierTransactionData.ImageData,
                ImageType = SupplierTransactionData.ImageType,

            };

            var newTransactionObj = await _transactionRepository.AddAsync(newTransaction);


            if (newTransactionObj == null) throw new InvalidOperationException("can not add Transaction");


            // => HandleTotalAmount
            var totalAmount = await _supplierTotalAmountService
                .SaveSupplierTotalAmountAsync(SupplierTransactionData.TransactionTypeId, SupplierTransactionData.Amount, SupplierTransactionData.SupplierId, userId);

            // craete Client Transaction
            var newSupplierTransactionObj = new SupplierTransaction
            {
                TransactionId = newTransactionObj!.TransactionId,
                UserId = userId,
                SupplierId = SupplierTransactionData.SupplierId,
            };

            var supplierTransactionAdded = await _supplierTransactionRepository.AddAsync(newSupplierTransactionObj);

            if (supplierTransactionAdded == null) throw new InvalidOperationException("Unable to add supplier transaction");

            // Default PaymentDate add in firsy time or just update totalAmount
            await _supplierPaymentDateService.SaveSupplierPaymentDateAsync(SupplierTransactionData.SupplierId, totalAmount, userId);

            return supplierTransactionAdded;

        }

		public async Task<bool> UpdateSupplierTransactionAsync(SupplierTransactionUpdateDto SupplierTransactionData)
		{
			// check is exist 
			var existSupplierTransaction = await _supplierTransactionRepository.GetByIdAsync(SupplierTransactionData.SupplierTransactionId);

			if (existSupplierTransaction == null) throw new KeyNotFoundException("Unable to find supplier transaction");

			// Handel Uploading Image
			if (SupplierTransactionData.ImageData == Array.Empty<byte>() && SupplierTransactionData.ImageType == string.Empty)
			{
				var ImageObj = await ImageHelper.HandelImageServices(SupplierTransactionData.FormImage!);

				SupplierTransactionData.ImageData = ImageObj.ImageData;
				SupplierTransactionData.ImageType = ImageObj.ImageType;
			}

			// update transaction only
			var transaction = await _transactionRepository.GetByIdAsync(existSupplierTransaction.TransactionId);
			var supplierTotalAmount = await _supplierTotalAmountService.GetSupplierTotalAmountBySupplierId(existSupplierTransaction.SupplierId);

			// calc total amount 
			var totalAmount = transaction.Amount;

			if (transaction.TransactionTypeId == (byte)enTransactionTypes.Payment) totalAmount -= supplierTotalAmount.TotalAmount;
			else if (transaction.TransactionTypeId == (byte)enTransactionTypes.Withdrawal) totalAmount += supplierTotalAmount.TotalAmount;


			transaction.ImageData = SupplierTransactionData.ImageData;
			transaction.ImageType = SupplierTransactionData.ImageType;
			transaction.Amount = totalAmount;
			transaction.Notes = SupplierTransactionData.Notes;
			transaction.TransactionDate = DateTime.Now;

			var isUpdatedes = await _transactionRepository.UpdateAsync(transaction);

			if (!isUpdatedes) throw new InvalidOperationException("Unable to updated client transaction");

			return false;
		}

		public async Task<bool> DeleteAsync(int supplierTransactionId, int userId)
        {
            var existSupplierTransaction = await _supplierTransactionRepository.GetByIdAsync(supplierTransactionId);

            if (existSupplierTransaction == null) throw new KeyNotFoundException($"there are not SupplierTransactionId = {existSupplierTransaction}");

            // Handle Total Amount Calculation
            var existSupplierTotalAmont = await _supplierTotalAmountService.GetSupplierTotalAmountBySupplierId(existSupplierTransaction.SupplierId);

            if (existSupplierTotalAmont == null) throw new KeyNotFoundException($"there are not supplier_total_amount record has clientId = {existSupplierTransaction.SupplierId}");


            var existTransaction = await _transactionRepository.GetByIdAsync(existSupplierTransaction.TransactionId);

            byte Payment = 1; byte Withdrawal = 2;

            existTransaction!.TransactionTypeId = existTransaction.TransactionTypeId == Payment ? Withdrawal : Payment;


            var totalAmount = await _supplierTotalAmountService.UpdateSupplierTotalAmountAsync(
                 existSupplierTotalAmont, existTransaction.Amount, existTransaction.TransactionTypeId);

            if (totalAmount <= 0) throw new InvalidOperationException("Invalid total amount calculated");


            // delete Supplier Transaction
            var supplierTransactionDeleted = await _supplierTransactionRepository.DeleteAsync(existSupplierTransaction.SupplierTransactionId);
            if (!supplierTransactionDeleted) throw new InvalidOperationException("Unable to delete supplier transaction");

            // delete Transaction
            var transactionDeleted = await _transactionRepository.DeleteAsync(existSupplierTransaction.TransactionId);
            if (!transactionDeleted) throw new InvalidOperationException("Unable to delete transaction");

            return true;
        }

        public async Task<SupplierTransaction> GetAsync(int supplierTransactionId)
        {
            var supplierTransaction = await _supplierTransactionRepository.GetByIdAsync(supplierTransactionId);

            if (supplierTransaction == null) throw new KeyNotFoundException($"there are no SupplierTransAction has this Id");

            return supplierTransaction;

        }


        public async Task<IEnumerable<SupplierTransaction>> GetAllAsync(int userId, int supplierId)
        {

            var supplierTransactions = await _supplierTransactionRepository.GetAllAsync(userId, supplierId);

            if (!supplierTransactions.Any()) throw new KeyNotFoundException($"there are no SupplierTransActions has suppliertId = {supplierId}");

            return supplierTransactions;

        }


    }
}
