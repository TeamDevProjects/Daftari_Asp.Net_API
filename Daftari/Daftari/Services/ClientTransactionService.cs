using Daftari.Data;
using Daftari.Dtos.Transactions.ClientTransactionDto;
using Daftari.Dtos.Transactions.UserTransactionDtos;
using Daftari.Entities;
using Daftari.Enums;
using Daftari.Interfaces;
using Daftari.Repositories;
using Daftari.Services.Images;
using Daftari.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services
{
    public class ClientTransactionService: IClientTransactionService
	{
        private readonly ITransactionRepository _transactionRepository;
        private readonly IClientTransactionRepository _clientTransactionRepository;

        private readonly IClientTotalAmountService _clientTotalAmountService;
        private readonly IClientPaymentDateService _clientPaymentDateService;

        public ClientTransactionService(DaftariContext context, IClientTotalAmountService clientTotalAmountService,
            IClientPaymentDateService clientPaymentDateService
            , ITransactionRepository transactionRepository, IClientTransactionRepository clientTransactionRepository)
        {
            _transactionRepository = transactionRepository;
            _clientTransactionRepository = clientTransactionRepository;

            _clientTotalAmountService = clientTotalAmountService;
            _clientPaymentDateService = clientPaymentDateService;
        }

        public async Task<ClientTransaction> AddClientTransactionAsync(ClientTransactionCreateDto clientTransactionData, int userId)
        {
            // Handle Uploading Image
            var imageObj = await ImageHelper.HandelImageServices(clientTransactionData.FormImage!);


            // Assign byte[] image data and type from the image handling service
            clientTransactionData.ImageData = imageObj.ImageData;
            clientTransactionData.ImageType = imageObj.ImageType;

            var newTransaction = new Transaction
            {
                TransactionTypeId = clientTransactionData.TransactionTypeId,
                Notes = clientTransactionData.Notes,
                TransactionDate = DateTime.Now,
                Amount = clientTransactionData.Amount,
                ImageData = clientTransactionData.ImageData,  // Byte array assigned here
                ImageType = clientTransactionData.ImageType,  // Image type (e.g., image/jpeg)
            };
            // Create Base Transaction
            var transactionAdded = await _transactionRepository.AddAsync(newTransaction);

            if (transactionAdded == null) throw new InvalidOperationException("Failed to add transaction");


            // Handle Total Amount Calculation
            var totalAmount = await _clientTotalAmountService.SaveClientTotalAmountAsync(
                clientTransactionData.TransactionTypeId, clientTransactionData.Amount, clientTransactionData.ClientId, userId);

            if (totalAmount <= 0) new InvalidOperationException("Invalid total amount calculated");


            // Create Client Transaction
            var newClientTransaction = new ClientTransaction
            {
                TransactionId = transactionAdded.TransactionId,
                UserId = userId,
                ClientId = clientTransactionData.ClientId,
            };

            var clientTransactionAdded = await _clientTransactionRepository.AddAsync(newClientTransaction);

            if (clientTransactionAdded == null) throw new InvalidOperationException("Unable to add ClientTransaction");

            // Handle Payment Date (add in the first time only)
            var paymentDateResult = await _clientPaymentDateService.SaveClientPaymentDateAsync(
                clientTransactionData.ClientId, totalAmount, userId);

            return clientTransactionAdded;
        }

		public async Task<bool> UpdateClientTransactionAsync(ClientTransactionUpdateDto ClientTransactionData)
		{
			// check is exist 
			var existClientTransaction = await _clientTransactionRepository.GetByIdAsync(ClientTransactionData.ClientTransactionId);

			if (existClientTransaction == null) throw new KeyNotFoundException("Unable to find user transaction");

			// Handel Uploading Image
			if (ClientTransactionData.ImageType == string.Empty)
			{
				var ImageObj = await ImageHelper.HandelImageServices(ClientTransactionData.FormImage!);

				ClientTransactionData.ImageData = ImageObj.ImageData;
				ClientTransactionData.ImageType = ImageObj.ImageType;
			}

			// update transaction only
			var transaction = await _transactionRepository.GetByIdAsync(existClientTransaction.TransactionId);
			var clientTotalAmount = await _clientTotalAmountService.GetClientTotalAmountByClientId(existClientTransaction.ClientId);

			// calc total amount 
			var oldAmount = transaction.Amount;
			decimal newAmount = 0;
			byte TransactionType = 0;

			newAmount = oldAmount - ClientTransactionData.Amount;

			if (transaction.TransactionTypeId == (byte)enTransactionTypes.Payment)
			{
				if (newAmount < 0) TransactionType = (byte)enTransactionTypes.Payment;

				else if (newAmount > 0) TransactionType = (byte)enTransactionTypes.Withdrawal;

				else TransactionType = transaction.TransactionTypeId;


			}
			else if (transaction.TransactionTypeId == (byte)enTransactionTypes.Withdrawal)
			{
				if (newAmount < 0) TransactionType = (byte)enTransactionTypes.Withdrawal;

				else if (newAmount > 0) TransactionType = (byte)enTransactionTypes.Payment;

				else TransactionType = transaction.TransactionTypeId;
			}

			newAmount = Math.Abs(newAmount);

			transaction.ImageData = ClientTransactionData.ImageData;
			transaction.ImageType = ClientTransactionData.ImageType;
			transaction.Amount = ClientTransactionData.Amount;
			transaction.Notes = ClientTransactionData.Notes;
			transaction.TransactionDate = DateTime.Now;

			var isUpdatedes = await _transactionRepository.UpdateAsync(transaction);

			await _clientTotalAmountService.UpdateClientTotalAmountAsync(clientTotalAmount, newAmount, TransactionType);

			if (!isUpdatedes) throw new InvalidOperationException("Unable to updated user transaction");

			return true;
		}

		public async Task<ClientTransaction> GetClientTransactionAsync(int clientTransactionId)
        {
            var clientTransaction = await _clientTransactionRepository.GetByIdAsync(clientTransactionId);

            if (clientTransaction == null) throw new KeyNotFoundException($"there are no ClientTransAction has this Id");


            return clientTransaction;

        }

        public async Task<IEnumerable<ClientTransaction>> GetClientTransactionsAsync(int userId, int clientId)
        {
            var clientTransactions = await _clientTransactionRepository.GetAllAsync(userId, clientId);

            if (!clientTransactions.Any()) new KeyNotFoundException($"there are no ClientTransActions has clientId = {clientId}");


            return clientTransactions;

        }


        public async Task<bool> DeleteClientTransactionAsync(int clientTransactionId, int userId)
        {
            var existClientTransaction = await _clientTransactionRepository.GetByIdAsync(clientTransactionId);

            if (existClientTransaction == null) throw new KeyNotFoundException($"there are not ClientTransactionId = {existClientTransaction}");

            // Handle Total Amount Calculation
            var existClientTotalAmont = await _clientTotalAmountService.GetClientTotalAmountByClientId(existClientTransaction.ClientId);

            if (existClientTotalAmont == null) throw new KeyNotFoundException($"there are not client_total_amount record has clientId = {existClientTransaction.ClientId}");


            var existTransaction = await _transactionRepository.GetByIdAsync(existClientTransaction.TransactionId);

            byte Payment = 1; byte Withdrawal = 2;

            existTransaction!.TransactionTypeId = existTransaction.TransactionTypeId == Payment ? Withdrawal : Payment;

            var totalAmount = await _clientTotalAmountService.UpdateClientTotalAmountAsync(
                existClientTotalAmont, existTransaction.Amount, existTransaction.TransactionTypeId);

            if (totalAmount <= 0) throw new InvalidOperationException("Invalid total amount calculated");


            // delete Client Transaction
            var isclientTransactiondeleted = await _clientTransactionRepository.DeleteAsync(existClientTransaction.ClientTransactionId);
            if (!isclientTransactiondeleted) throw new InvalidOperationException("Unable to delete client transaction");

            // delete Transaction
            var isTransactiondeleted = await _transactionRepository.DeleteAsync(existClientTransaction.TransactionId);
            if (!isTransactiondeleted) throw new InvalidOperationException("Unable to delete transaction");

            return true;
        }

    }
}
