using Daftari.Data;
using Daftari.Dtos.PaymentDates.Bases;
using Daftari.Dtos.PaymentDates.ClientPaymentDateDtos;
using Daftari.Services;
using Daftari.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
    [Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ClientPaymentDatesController : BaseController
	{
		private readonly IClientPaymentDateService _clientPaymentDateService;
		private readonly IPaymentDateService _paymentDateService;
		private readonly IClientTotalAmountService _clientTotalAmountService;
		public ClientPaymentDatesController(DaftariContext context

			, IClientPaymentDateService clientPaymentDateService,IPaymentDateService paymentDateService, IClientTotalAmountService clientTotalAmountService) : base(context)
		{
			_clientPaymentDateService = clientPaymentDateService;
			_paymentDateService = paymentDateService;
			_clientTotalAmountService = clientTotalAmountService;
		}


		// Add +
		[HttpPost]
		public async Task<IActionResult> AddClientPaymentDate([FromBody] ClientPaymentDateBaseDto dto)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");

				var existClientPaymentDate = await _clientPaymentDateService.GetClientPaymentDateByClientIdAsync(dto.ClientId);//Exist function
			
				if (existClientPaymentDate != null) return BadRequest("there are Payment already you can`t add anther one");


				var clientTotalAmount = await _clientTotalAmountService.GetClientTotalAmountByClientId(dto.ClientId);


				var clientPaymentDateAdded = await _clientPaymentDateService.AddClientPaymentDateAsync(
					new ClientPaymentDateCreateDto
					{
						DateOfPayment = dto.DateOfPayment,// default no
						TotalAmount = clientTotalAmount.TotalAmount,
						PaymentMethodId = 1,
						Notes = "this PaymentDate is added by User",
						UserId = userId,
						ClientId = dto.ClientId,
					});


				await transaction.CommitAsync();

				return Ok(clientPaymentDateAdded);

			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}

		// Update
		[HttpPut("{clientPaymentDateId}")]
		public async Task<IActionResult> UpdateClientPaymentDate(int clientPaymentDateId, [FromBody] ClientPaymentDateBaseDto dto)
		{
			if (dto == null || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Use 'await using' for automatic transaction disposal
			await using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				var existClientPaymentDate = await _clientPaymentDateService.GetClientPaymentDateByIdAsync(clientPaymentDateId);

				var userId = GetUserIdFromToken();

				if (userId == -1 || userId != existClientPaymentDate.UserId)
				{
					return Unauthorized("UserId not found in token or does not match.");
				}

				// Call the service to update the date of payment
				await _paymentDateService.UpdateDatePaymentDateAsync(existClientPaymentDate.PaymentDateId, dto.DateOfPayment,dto.Notes);

				await transaction.CommitAsync();

				return Ok("Date of payment updated successfully.");
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		// Get
		[HttpGet("{clientPaymentDateId}")]
		public async Task<IActionResult> GetClientPaymentDateById(int clientPaymentDateId)
		{
			try { 

				var existClientPaymentDate = await _clientPaymentDateService.GetClientPaymentDateByIdAsync(clientPaymentDateId);

				return Ok(existClientPaymentDate);
			}catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message );
			
			}catch (Exception ex) 
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		[HttpGet("clientId/{clientId}")]
		public async Task<IActionResult> GetClientPaymentDateByClientId(int clientId)
		{
			try
			{
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var existClientPaymentDate = await _clientPaymentDateService.GetClientPaymentDateByClientIdAsync(clientId);

				return Ok(existClientPaymentDate);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) 
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		// Delete
		[HttpDelete("{clientPaymentDateId}")]
		public async Task<IActionResult> DeleteClientPaymentDateByUserId(int clientPaymentDateId)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				var existClientPaymentDate = await _clientPaymentDateService.GetClientPaymentDateByIdAsync(clientPaymentDateId);
				
				var userId = GetUserIdFromToken();

				if (userId == -1 || userId != existClientPaymentDate.UserId)
				{
					return Unauthorized("UserId is not founded in token");
				}


				var isDeleted = await _clientPaymentDateService.DeleteClientPaymentDateAsync(existClientPaymentDate.ClientPaymentDateId);

				if (!isDeleted) return BadRequest("Unable to delete client payment date");

				await transaction.CommitAsync();

				return Ok(" client_payment_date deleted Succefuly");
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

			}
		}

	}
}




