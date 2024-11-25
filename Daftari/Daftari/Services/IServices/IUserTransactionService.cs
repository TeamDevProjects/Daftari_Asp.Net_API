using Daftari.Dtos.Transactions.UserTransactionDtos;
using Daftari.Entities;
using Daftari.Services.Images;

namespace Daftari.Services.IServices
{
    public interface IUserTransactionService
	{
		Task<UserTransaction> AddUserTransactionAsync(UserTransactionCreateDto UserTransactionData, int userId);

		Task<bool> UpdateUserTransactionAsync(UserTransactionUpdateDto UserTransactionData);

		Task<bool> DeleteUserTransactionAsync(int userTransactionId, int userId);

		Task<UserTransaction> GetUserTransactionAsync(int userTransactionId);

		Task<IEnumerable<UserTransaction>> GetUserTransactionsAsync(int userId);
	}
}
