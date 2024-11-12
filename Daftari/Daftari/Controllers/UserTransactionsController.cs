using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Daftari.Helper;
using Daftari.Services.Images;
using Daftari.Services.TotalAmountServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserTransactionsController : BaseTransactionsController
	{
		private readonly UserTotalAmountService _userTotalAmountService;
		public UserTransactionsController(DaftariContext context, JwtHelper jwtHelper , UserTotalAmountService userTotalAmountService)
			: base(context, jwtHelper)
		{
			_userTotalAmountService = userTotalAmountService;
		}


		// Add     +
		// Update  
		// Delete  
		// Get => view

		

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

				// => Add/updateTotalAmount
				var totalAmount = await _userTotalAmountService.SaveUserTotalAmount(UserTransactionData.TransactionTypeId, UserTransactionData.Amount, userId);

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
