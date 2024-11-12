using Daftari.Data;
using Daftari.Entities;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services.TotalAmountServices
{
	public class ClientTotalAmountService
	{
		private readonly DaftariContext _context;

		public ClientTotalAmountService(DaftariContext context)
		{
			_context = context;

		}

		private async Task AddClientTotalAmount(decimal totalAmount, int userId, int clientId)
		{
			var newClientTotalAmount = new ClientTotalAmount
			{
				UpdateAt = DateTime.UtcNow,
				UserId = userId,
				ClientId = clientId,
				TotalAmount = totalAmount
			};

			await _context.ClientTotalAmounts.AddAsync(newClientTotalAmount);
			await _context.SaveChangesAsync();
		}

		private async Task<decimal> UpdateClientTotalAmount(ClientTotalAmount existClientTotalAmount, decimal Amount, byte TransactionTypeId)
		{
			try
			{
				decimal totalAmount = Amount;

				if (TransactionTypeId == 1)
				{
					totalAmount = existClientTotalAmount.TotalAmount - Amount;

					existClientTotalAmount.TotalAmount = totalAmount;
				}
				else if (TransactionTypeId == 2)
				{
					totalAmount = existClientTotalAmount.TotalAmount + Amount;

					existClientTotalAmount.TotalAmount = totalAmount;
				}

				existClientTotalAmount.UpdateAt = DateTime.UtcNow; // Update timestamp

				_context.ClientTotalAmounts.Update(existClientTotalAmount);
				await _context.SaveChangesAsync();

				return totalAmount;
			}
			catch (Exception ex) { throw ex; }
		}


		public async Task<decimal> SaveClientTotalAmount(byte TransactionTypeId, decimal Amount, int clientId, int userId)
		{
			decimal totalAmount = Amount;

			try
			{
				var existUserTotalAmount = await _context.ClientTotalAmounts
					.FirstOrDefaultAsync(c => c.UserId == userId);

				if (existUserTotalAmount == null)
				{
					await AddClientTotalAmount(totalAmount, userId, clientId);
				}
				else
				{
					totalAmount = await UpdateClientTotalAmount(existUserTotalAmount, Amount, TransactionTypeId);
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
