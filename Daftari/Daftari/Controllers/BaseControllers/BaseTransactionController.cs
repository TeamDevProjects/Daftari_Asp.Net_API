using Daftari.Data;
using Daftari.Entities;
using Daftari.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Daftari.Controllers.BaseControllers
{
	public class BaseTransactionController : BaseController
	{
		public BaseTransactionController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
		{
		}

		protected async Task<Transaction> CreateTransactionAsync(Transaction transaction)
		{
			if (transaction == null)
			{
				throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
			}

			try
			{
				await _context.Transactions.AddAsync(transaction);
				await _context.SaveChangesAsync();
				return transaction;
			}
			catch (DbUpdateException ex)
			{
				// Log the exception (logging mechanism not shown)
				throw new Exception("An error occurred while saving the transaction. Please try again.", ex);
			}
		}

		// Method to get a transaction by its ID
		protected async Task<Transaction> GetTransactionByIdAsync(int transactionId)
		{
			var transaction = await _context.Transactions
											.AsNoTracking() // Improves performance for read-only operations
											.FirstOrDefaultAsync(t => t.TransactionId == transactionId);

			if (transaction == null)
			{
				throw new KeyNotFoundException($"Transaction with ID {transactionId} was not found.");
			}

			return transaction;
		}

		// Method to get all transactions
		protected async Task<List<Transaction>> GetAllTransactionsAsync()
		{
			return await _context.Transactions
								 .AsNoTracking() // Improves performance for read-only operations
								 .ToListAsync();
		}

		// Method to get transactions filtered by user ID
		protected async Task<List<Transaction>> GetTransactionsByUserIdAsync(int TransactionTypeId)
		{
			return await _context.Transactions
								 .AsNoTracking()
								 .Where(t => t.TransactionTypeId == TransactionTypeId)
								 .ToListAsync();
		}
	}
}
