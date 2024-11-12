using Daftari.Data;
using Daftari.Entities;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services.TotalAmountServices
{
	public class SupplierTotalAmountService
	{
		private readonly DaftariContext _context;

		public SupplierTotalAmountService(DaftariContext context)
		{
			_context = context;

		}

		private async Task AddSupplierTotalAmount(decimal totalAmount, int userId,int supplierId)
		{
			var newSupplierTotalAmount = new SupplierTotalAmount
			{
				UpdateAt = DateTime.UtcNow,
				UserId = userId,
				SupplierId = supplierId,
				TotalAmount = totalAmount
			};

			await _context.SupplierTotalAmounts.AddAsync(newSupplierTotalAmount);
			await _context.SaveChangesAsync();
		}

		private async Task<decimal> UpdateSupplierTotalAmount(SupplierTotalAmount existSupplierTotalAmount, decimal Amount, byte TransactionTypeId)
		{
			try
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

				_context.SupplierTotalAmounts.Update(existSupplierTotalAmount);
				await _context.SaveChangesAsync();

				return totalAmount;
			}
			catch (Exception ex) { throw ex; }
		}


		public async Task<decimal> SaveSupplierTotalAmount(byte TransactionTypeId, decimal Amount,int supplierId, int userId)
		{
			decimal totalAmount = Amount;

			try
			{
				var existUserTotalAmount = await _context.SupplierTotalAmounts
					.FirstOrDefaultAsync(c => c.UserId == userId);

				if (existUserTotalAmount == null)
				{
					await AddSupplierTotalAmount(totalAmount, userId, supplierId);
				}
				else
				{
					totalAmount = await UpdateSupplierTotalAmount(existUserTotalAmount, Amount, TransactionTypeId);
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
