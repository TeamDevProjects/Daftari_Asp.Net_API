using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Daftari.Helper;
using Daftari.Services.Images;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserTransactionController : BaseTransactionController
	{

		public UserTransactionController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
		{
		}


		// Add     +
		// Update  
		// Delete  
		// Get => view

		private async Task<decimal> SaveUserTotalAmount(UserTransactionCreateDto userTransactionData, int userId)
		{
			decimal totalAmount = userTransactionData.Amount;

			try
			{
				var existUserTotalAmount = await _context.UserTotalAmounts
					.FirstOrDefaultAsync(c =>  c.UserId == userId);

				if (existUserTotalAmount == null)
				{
					totalAmount = userTransactionData.Amount;
					// Create new User TotalAmount
					var newUserTotalAmount = new UserTotalAmount
					{
						UpdateAt = DateTime.UtcNow,
						UserId = userId,
						TotalAmount = totalAmount
					};

					await _context.UserTotalAmounts.AddAsync(newUserTotalAmount);
					await _context.SaveChangesAsync();
				}
				else
				{
					if (userTransactionData.TransactionTypeId == 1)
					{
						totalAmount = existUserTotalAmount.TotalAmount - userTransactionData.Amount;

						existUserTotalAmount.TotalAmount = totalAmount;
					}
					else if (userTransactionData.TransactionTypeId == 2)
					{
						totalAmount = existUserTotalAmount.TotalAmount + userTransactionData.Amount;

						existUserTotalAmount.TotalAmount = totalAmount;
					}

					existUserTotalAmount.UpdateAt = DateTime.UtcNow; // Update timestamp

					_context.UserTotalAmounts.Update(existUserTotalAmount);
					await _context.SaveChangesAsync();

				}
			}
			catch (Exception ex)
			{
				// Log the exception here, if necessary
				throw;
			}

			return totalAmount;
		}


		[HttpPost]
		public async Task<IActionResult> CreateUserTransaction([FromForm] UserTransactionCreateDto UserTransactionData)
		{
			// Handel Uploading Image
			var ImageObj = await ImageServices.HandelImageServices(UserTransactionData.FormImage!);

			UserTransactionData.ImageData = ImageObj.ImageData;
			UserTransactionData.ImageType = ImageObj.ImageType;


			// Get UserId from header request from token
			var userId = GetUserIdFromToken();

			if (userId == -1)
			{
				return Unauthorized("UserId is not founded in token");
			}

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{

				// create Base Transaction
				var newTransactionObj = await CreateTransactionAsync(
					new Transaction
					{
						TransactionTypeId = UserTransactionData.TransactionTypeId,
						Notes = UserTransactionData.Notes,
						TransactionDate = DateTime.Now,
						Amount = UserTransactionData.Amount,
						ImageData = UserTransactionData.ImageData,
						ImageType = UserTransactionData.ImageType,

					});

				if (newTransactionObj == null)
				{
					return BadRequest("can not add Transaction");
				}

				// => HandleTotalAmount
				var totalAmount = await SaveUserTotalAmount(UserTransactionData, userId);

				// craete User Transaction
				var newUserTransactionObj = new UserTransaction
				{
					TransactionId = newTransactionObj!.TransactionId,
					UserId = userId,
					TotalAmount = totalAmount

				};

				_context.UserTransactions.Add(newUserTransactionObj);
				await _context.SaveChangesAsync();


				await transaction.CommitAsync();
				return Ok("Transaction Created Succfuly");
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}
	}
}
