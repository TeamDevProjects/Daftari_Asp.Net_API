using Daftari.Data;
using Daftari.Entities;
using Daftari.Enums;
using Daftari.Interfaces;
using Daftari.Services.Images;
using Daftari.Services.IServices;
using Daftari.Dtos.Transactions.UserTransactionDtos;
using Daftari.Entities.Views;
using Microsoft.AspNetCore.Authorization;

namespace Daftari.Services
{
	[Authorize]
	public class UserTransactionService: IUserTransactionService
	{
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserTransactionRepository _userTransactionRepository;

        private readonly IUserTotalAmountService _userTotalAmountService;


        
		public UserTransactionService(DaftariContext context, IUserTotalAmountService userTotalAmountService
            , IUserTransactionRepository userTransactionRepository, ITransactionRepository transactionRepository)
        {
            _userTotalAmountService = userTotalAmountService;
            _transactionRepository = transactionRepository;
            _userTransactionRepository = userTransactionRepository;
        }

        public async Task<UserTransaction> AddUserTransactionAsync(UserTransactionCreateDto UserTransactionData, int userId)
        {
            // Handel Uploading Image
            var ImageObj = await ImageHelper.HandelImageServices(UserTransactionData.FormImage!);

            UserTransactionData.ImageData = ImageObj.ImageData;
            UserTransactionData.ImageType = ImageObj.ImageType;


            // create Base Transaction
            var newTransaction = new Transaction
            {
                TransactionTypeId = UserTransactionData.TransactionTypeId,
                Notes = UserTransactionData.Notes,
                TransactionDate = DateTime.Now,
                Amount = UserTransactionData.Amount,
                ImageData = UserTransactionData.ImageData,
                ImageType = UserTransactionData.ImageType,
            };

            var newTransactionObj = await _transactionRepository.AddAsync(newTransaction);

            if (newTransactionObj == null) throw new InvalidOperationException("Unable to create Transaction");

            // => Add/updateTotalAmount
            await _userTotalAmountService.SaveAsync(UserTransactionData.TransactionTypeId, UserTransactionData.Amount, userId);

            // craete User Transaction
            var newUserTransactionObj = new UserTransaction
            {
                TransactionId = newTransactionObj!.TransactionId,
                UserId = userId,
            };

            var userTransactionAdded = await _userTransactionRepository.AddAsync(newUserTransactionObj);
            if (userTransactionAdded == null) throw new InvalidOperationException("Unable to create Transaction");

            return userTransactionAdded;
        }

        public async Task<bool> DeleteUserTransactionAsync(int userTransactionId, int userId)
        {
            var existUserTransaction = await _userTransactionRepository.GetByIdAsync(userTransactionId);

            if (existUserTransaction == null) throw new KeyNotFoundException($"there are no UserTransactionId = {existUserTransaction}");

            // Handle Total Amount Calculation
            var existUserTotalAmont = await _userTotalAmountService.GetTotalAmountByUserId(userId);

            if (existUserTotalAmont == null) { throw new KeyNotFoundException($"there are no user_total_amount record has clientId = {userId}"); }


            var existTransaction = await _transactionRepository.GetByIdAsync(existUserTransaction.TransactionId);

            existTransaction!.TransactionTypeId = existTransaction.TransactionTypeId == (byte)enTransactionTypes.Payment 
                ? (byte)enTransactionTypes.Withdrawal : (byte)enTransactionTypes.Payment;

            var totalAmount = await _userTotalAmountService.UpdateAsync(
                 existUserTotalAmont, existTransaction.Amount, existTransaction.TransactionTypeId);

            if (totalAmount <= 0) throw new InvalidOperationException("Invalid total amount calculated");

            // delete Supplier Transaction
            var userTransactionDeleted = await _userTransactionRepository.DeleteAsync(existUserTransaction.UserTransactionId);
            if (!userTransactionDeleted) throw new InvalidOperationException("Unableto delete user transaction");
            // delete Transaction
            var transactionDeleted = await _transactionRepository.DeleteAsync(existUserTransaction.TransactionId);
            if (!transactionDeleted) throw new InvalidOperationException("Unableto delete  transaction");

            return true;
        }

        public async Task<bool> UpdateUserTransactionAsync(UserTransactionUpdateDto UserTransactionData)
        {
            // check is exist 
            var existUserTransaction = await _userTransactionRepository.GetByIdAsync(UserTransactionData.UserTransactionId);
            
            if (existUserTransaction == null) throw new KeyNotFoundException("Unable to find user transaction");
            
            // Handel Uploading Image
            if (UserTransactionData.ImageData == Array.Empty<byte>() && UserTransactionData.ImageType == string.Empty)
            {
                var ImageObj = await ImageHelper.HandelImageServices(UserTransactionData.FormImage!);

                UserTransactionData.ImageData = ImageObj.ImageData;
                UserTransactionData.ImageType = ImageObj.ImageType;
            }

            // update transaction only
            var transaction = await _transactionRepository.GetByIdAsync(existUserTransaction.TransactionId);
            var userTotalAmount = await _userTotalAmountService.GetTotalAmountByUserId(existUserTransaction.UserId);

			// calc total amount 
			var oldAmount = transaction.Amount;
            decimal newAmount = 0;
            byte TransactionType = 0;                                                                

            newAmount = oldAmount - UserTransactionData.Amount;
            
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

			transaction.ImageData = UserTransactionData.ImageData;
			transaction.ImageType = UserTransactionData.ImageType;
            transaction.Amount = UserTransactionData.Amount;
			transaction.Notes = UserTransactionData.Notes;
            transaction.TransactionDate = DateTime.Now;

            var isUpdatedes = await _transactionRepository.UpdateAsync(transaction);

			await _userTotalAmountService.UpdateAsync(userTotalAmount, newAmount, TransactionType);

			if (!isUpdatedes) throw new InvalidOperationException("Unable to updated user transaction");

			return true;
        }

		public async Task<UserTransaction> GetUserTransactionAsync(int userTransactionId)
        {
            var userTransaction = await _userTransactionRepository.GetByIdAsync(userTransactionId);

            if (userTransaction == null) new KeyNotFoundException($"there are no UserTransAction has this Id");

            return userTransaction;
		}

        public async Task<IEnumerable<UserTransactionsView>> GetUserTransactionsAsync(int userId)
        {
            var userTransactions = await _userTransactionRepository.GetAllAsync(userId);

            if (!userTransactions.Any()) throw new KeyNotFoundException($"there are no UserTransActions has userId = {userId}");

            return userTransactions;

        }



    }

}
