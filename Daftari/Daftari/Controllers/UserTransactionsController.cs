using Daftari.Data;
using Daftari.Dtos.Transactions.UserTransactionDtos;
using Daftari.Entities;
using Daftari.Services;
using Daftari.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    [Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserTransactionsController : BaseController
	{
		private readonly IUserTransactionService _userTransactionService;
		public UserTransactionsController(DaftariContext context ,IUserTransactionService userTransactionService)
			: base(context)
		{
			_userTransactionService = userTransactionService;
		}


		[HttpPost]
		public async Task<IActionResult> CreateUserTransaction([FromForm] UserTransactionCreateDto UserTransactionData)
		{

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				// create Base Transaction
				var userTransaction = await _userTransactionService.AddUserTransactionAsync(UserTransactionData, userId);

				await transaction.CommitAsync();
				return Ok(userTransaction);
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateUserTransaction([FromForm] UserTransactionUpdateDto UserTransactionData)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				// create Base Transaction
				var userTransaction = await _userTransactionService.UpdateUserTransactionAsync(UserTransactionData);

				await transaction.CommitAsync();
				return Ok(userTransaction);
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}
		

		[HttpDelete("{userTransactionId}")]
		public async Task<IActionResult> DeleteUserTransaction(int userTransactionId)
		{
			
			

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1)
				{
					return Unauthorized("UserId is not found in token");
				}

				await _userTransactionService.DeleteUserTransactionAsync(userTransactionId,userId);

				// Commit transaction
				await transaction.CommitAsync();
				return Ok("Client Transaction deleted successfully");
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}


		[HttpGet("{userTransactionId}")]
		public async Task<ActionResult<UserTransaction>> GetUserTransActionsbyId(int userTransactionId)
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not found in token");

				var userTransaction = await _userTransactionService.GetUserTransactionAsync(userTransactionId);

				return Ok(userTransaction);

			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserTransaction>>> GetAllUserTransActions()
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not found in token");

				var userTransactions = await _userTransactionService.GetUserTransactionsAsync(userId);
				
				return Ok(userTransactions);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}


	}
}
