using Daftari.Data;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services.TotalAmountServices
{
	public class UserTotalAmountService
	{
		private readonly DaftariContext _context;

		public UserTotalAmountService(DaftariContext context)
        {
			_context = context;

		}

		private async Task AddUserTotalAmount(decimal totalAmount, int userId)
		{
			var newUserTotalAmount = new UserTotalAmount
			{
				UpdateAt = DateTime.UtcNow,
				UserId = userId,
				TotalAmount = totalAmount
			};

			await _context.UserTotalAmounts.AddAsync(newUserTotalAmount);
			await _context.SaveChangesAsync();
		}

		private async Task<decimal> UpdateUserTotalAmount(UserTotalAmount existUserTotalAmount, decimal Amount, byte TransactionTypeId)
		{
			try
			{
				decimal totalAmount = Amount;

				if (TransactionTypeId == 1)
				{
					totalAmount = existUserTotalAmount.TotalAmount - Amount;

					existUserTotalAmount.TotalAmount = totalAmount;
				}
				else if (TransactionTypeId == 2)
				{
					totalAmount = existUserTotalAmount.TotalAmount + Amount;

					existUserTotalAmount.TotalAmount = totalAmount;
				}

				existUserTotalAmount.UpdateAt = DateTime.UtcNow; // Update timestamp

				_context.UserTotalAmounts.Update(existUserTotalAmount);
				await _context.SaveChangesAsync();

				return totalAmount;
			}
			catch (Exception ex) { throw ex; }
		}


		public async Task<decimal> SaveUserTotalAmount(byte TransactionTypeId,decimal Amount, int userId)
		{
			decimal totalAmount = Amount;

			try
			{
				var existUserTotalAmount = await _context.UserTotalAmounts
					.FirstOrDefaultAsync(c => c.UserId == userId);

				if (existUserTotalAmount == null)
				{
					await AddUserTotalAmount(totalAmount,userId);
				}
				else
				{
					totalAmount = await UpdateUserTotalAmount(existUserTotalAmount, Amount,TransactionTypeId);
				}
			}
			catch (Exception ex)
			{
				// Log the exception here, if necessary
				throw ex;
			}

			return totalAmount;
		}
	}
}
