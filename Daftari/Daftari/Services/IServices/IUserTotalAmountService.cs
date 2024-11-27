using Daftari.Entities;

namespace Daftari.Services.IServices
{
	public interface IUserTotalAmountService
	{
		Task<decimal> AddAsync(decimal totalAmount, int userId);

		Task<decimal> UpdateAsync(UserTotalAmount existUserTotalAmount, decimal Amount, byte TransactionTypeId);

		Task<decimal> SaveAsync(byte TransactionTypeId, decimal Amount, int userId);

		Task<UserTotalAmount> GetTotalAmountByUserId(int userId);
	}
}
