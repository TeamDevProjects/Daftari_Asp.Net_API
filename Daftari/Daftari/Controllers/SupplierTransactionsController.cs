using Daftari.Data;
using Daftari.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Daftari.Services;
using Daftari.Dtos.Transactions.SupplierTransactionDtos;
using Daftari.Services.IServices;

namespace Daftari.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class SupplierTransactionsController : BaseController
	{
		private readonly ISupplierTransactionService _supplierTransactionService;

		public SupplierTransactionsController(
			DaftariContext context, 
			ISupplierTransactionService supplierTransactionService)
			: base(context)
		{
			_supplierTransactionService = supplierTransactionService;
		}



		[HttpPost]
		public async Task<IActionResult> CreateSupplierTransaction([FromForm] SupplierTransactionCreateDto SupplierTransactionData)
		{
			// Get UserId from header request from token
			var userId = GetUserIdFromToken();

			if (userId == -1)
			{
				return Unauthorized("UserId is not founded in token");
			}

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				var supplierTransaction = await _supplierTransactionService.AddSupplierTransactionAsync(SupplierTransactionData, userId);

				await transaction.CommitAsync();
				return Ok(supplierTransaction);
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
		public async Task<IActionResult> UpdateSupplierTransaction([FromForm] SupplierTransactionUpdateDto SupplierTransactionData)
		{
			// Get UserId from header request from token
			var userId = GetUserIdFromToken();

			if (userId == -1)
			{
				return Unauthorized("UserId is not founded in token");
			}

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				var supplierTransaction = await _supplierTransactionService.UpdateSupplierTransactionAsync(SupplierTransactionData);

				await transaction.CommitAsync();
				return Ok(supplierTransaction);
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


		[HttpDelete("{supplierTransactionId}")]
		public async Task<IActionResult> DeleteSupplierTransaction(int supplierTransactionId)
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
				await _supplierTransactionService.DeleteAsync(supplierTransactionId, userId);

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


		[HttpGet("{supplierTransactionId}")]
		public async Task<ActionResult<SupplierTransaction>> GetSupplierTransActionsbyId(int supplierTransactionId)
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not found in token");
				}

				var supplierTransaction = await _supplierTransactionService.GetAsync(supplierTransactionId);

				return Ok(supplierTransaction);

			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message );
			}catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}


		[HttpGet("supplierId/{supplierId}")]
		public async Task<ActionResult<IEnumerable<SupplierTransaction>>> GetAllSupplierTransActions(int supplierId)
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not found in token");
				}

				var supplierTransactions = await _supplierTransactionService.GetAllAsync(userId,supplierId);

				return Ok(supplierTransactions);

			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}

	}
}
