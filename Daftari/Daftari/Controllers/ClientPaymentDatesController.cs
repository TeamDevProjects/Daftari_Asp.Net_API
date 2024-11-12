using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.PaymentDates.Bases;
using Daftari.Dtos.PaymentDates.ClientPaymentDateDtos;
using Daftari.Entities;
using Daftari.Helper;
using Daftari.Services.PaymentDateServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientPaymentDatesController : BaseController
	{
		private readonly ClientPaymentDateService _clientPaymentDateService;
		public ClientPaymentDatesController(DaftariContext context, JwtHelper jwtHelper
			, ClientPaymentDateService clientPaymentDateService) : base(context, jwtHelper)
		{
			_clientPaymentDateService = clientPaymentDateService;

		}


		// Add
		[HttpPost]
		public async Task<IActionResult> AddClientPaymentDate(ClientPaymentDateBaseDto dto)
		{
			try
			{
				var userId = GetUserIdFromToken();

				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var existClientPaymentDate = await _context.ClientPaymentDates
					.FirstOrDefaultAsync(c => c.ClientId == dto.ClientId && c.UserId == userId);

				if (existClientPaymentDate != null)
				{
					return BadRequest("This client payment date is exist you can not add it again");
				}

				var clientPaymentDate = await _clientPaymentDateService.CreateClientPaymentDateAsync(
					new ClientPaymentDateCreateDto
					{
						DateOfPayment = dto.DateOfPayment,// default no
						TotalAmount = dto.TotalAmount,
						PaymentMethodId = 1,
						Notes = "this PaymentDate is added by User",
						UserId = userId,
						ClientId = dto.ClientId,
					});

				return Ok("Payment_date created Succefuly");

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}

		// Update
		[HttpPut("{clientPaymentDateId}")]
		public async Task<IActionResult> UpdateClientPaymentDate(ClientPaymentDateBaseDto dto, int clientPaymentDateId)
		{
			try
			{
				var existClientPaymentDate = await _context.ClientPaymentDates.FindAsync(clientPaymentDateId);

				if (existClientPaymentDate == null)
				{
					return BadRequest($"This client_payment_date = {clientPaymentDateId} is not exist ");
				}


				await _clientPaymentDateService.UpdateDateOfBaymentAsync(clientPaymentDateId, dto.DateOfPayment);

				return Ok("date of payment_date updated Succefuly");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

			}

		}
		
		// Get
		[HttpGet("{clientPaymentDateId}")]
		public async Task<IActionResult> GetClientPaymentDateById(int clientPaymentDateId)
		{
			try { 

				var existClientPaymentDate = await _context.ClientPaymentDates.FindAsync(clientPaymentDateId);

				if (existClientPaymentDate == null)
				{
					return BadRequest($"This client_payment_date = {clientPaymentDateId} is not exist ");
				}

				return Ok(existClientPaymentDate);
			}catch (Exception ex) 
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		[HttpGet("clientId/{clientId}")]
		public async Task<IActionResult> GetClientPaymentDateByUserId(int clientId)
		{
			try
			{
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var existClientPaymentDate = await _context.ClientPaymentDates.FirstOrDefaultAsync((c)=> c.ClientId == clientId && c.UserId == userId);

				if (existClientPaymentDate == null)
				{
					return BadRequest($"This client_payment_date clientId = {clientId} is not exist ");
				}

				return Ok(existClientPaymentDate);
			}
			catch (Exception ex) 
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		// Delete
		[HttpDelete]
		public async Task<IActionResult> DeleteClientPaymentDateByUserId(int clientPaymentDateId)
		{
			try
			{
				var existClientPaymentDate = await _context.ClientPaymentDates.FindAsync(clientPaymentDateId);

				if (existClientPaymentDate == null)
				{
					return BadRequest($"This client_payment_date = {clientPaymentDateId} is not exist ");
				}

				_context.ClientPaymentDates.Remove(existClientPaymentDate);
				await _context.SaveChangesAsync();

				return Ok(" client_payment_date deleted Succefuly");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

			}
		}

	}
}




