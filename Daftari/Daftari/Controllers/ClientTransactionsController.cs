using Daftari.Data;
using Daftari.Dtos.Transactions.ClientTransactionDto;
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
	public class ClientTransactionsController : BaseController
	{
		private readonly IClientTransactionService _clientTransactionService;

		public ClientTransactionsController(
			DaftariContext context,

			IClientTransactionService clientTransactionService)
			: base(context)
		{
			_clientTransactionService = clientTransactionService;
		}



		[HttpPost] 
		public async Task<IActionResult> CreateClientTransaction([FromForm] ClientTransactionCreateDto clientTransactionData)
		{

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not found in token");

				var clientTransaction = await _clientTransactionService.AddClientTransactionAsync(clientTransactionData, userId);

				// Commit transaction
				await transaction.CommitAsync();
				return Ok(clientTransaction);
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return NotFound(ex.Message);
			}catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return BadRequest(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateClientTransaction([FromForm] ClientTransactionUpdateDto clientTransactionData)
		{

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not found in token");

				var clientTransaction = await _clientTransactionService.UpdateClientTransactionAsync(clientTransactionData);

				// Commit transaction
				await transaction.CommitAsync();
				return Ok(clientTransaction);
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}


		[HttpGet("{clientTransactionId}")]
		public async Task<ActionResult<ClientTransaction>> GetClientTransActionsbyId(int clientTransactionId)
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not found in token");
				}

				var clientTransaction = await _clientTransactionService.GetClientTransactionAsync(clientTransactionId);

				return Ok(clientTransaction);
			
			
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message) ;	
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

			}

		}


		[HttpGet("clientId/{clientId}")]
		public async Task<ActionResult<IEnumerable<ClientTransaction>>> GetAllClientTransActions(int clientId)
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not found in token");
				}

				var clientTransactions = await _clientTransactionService.GetClientTransactionsAsync(userId,clientId);

				return Ok(clientTransactions);

			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);

			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

			}

		}



		[HttpDelete("{clientTransactionId}")]
		public async Task<IActionResult> DeleteClientTransaction(int clientTransactionId)
		{
			
			// Get UserId from header request from token
			var userId = GetUserIdFromToken();

			if (userId == -1)
			{
				return Unauthorized("UserId is not found in token");
			}

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				await _clientTransactionService.DeleteClientTransactionAsync(clientTransactionId, userId);

				// Commit transaction
				await transaction.CommitAsync();
				return Ok("Client Transaction deleted successfully");
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return NotFound(ex.Message);
			}catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return BadRequest(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}


	}
}

	