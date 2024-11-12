using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.PaymentDates.Bases;
using Daftari.Dtos.PaymentDates.SupplierPaymentDateDtos;
using Daftari.Helper;
using Daftari.Services.PaymentDateServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SupplierPaymentDatesController : BaseController
	{
		private readonly SupplierPaymentDateService _supplierPaymentDateService;
		public SupplierPaymentDatesController(DaftariContext context, JwtHelper jwtHelper
			, SupplierPaymentDateService supplierPaymentDateService) : base(context, jwtHelper)
		{
			_supplierPaymentDateService = supplierPaymentDateService;

		}


		// Add
		[HttpPost]
		public async Task<IActionResult> AddSupplierPaymentDate(SupplierPaymentDateBaseDto dto)
		{
			try
			{
				var userId = GetUserIdFromToken();

				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var existSupplierPaymentDate = await _context.SupplierPaymentDates
					.FirstOrDefaultAsync(c => c.SupplierId == dto.SupplierId && c.UserId == userId);

				if (existSupplierPaymentDate != null)
				{
					return BadRequest("This Supplier payment date is exist you can not add it again");
				}

				var clientPaymentDate = await _supplierPaymentDateService.CreateSupplierPaymentDateAsync(
					new SupplierPaymentDateCreateDto
					{
						DateOfPayment = dto.DateOfPayment,// default no
						TotalAmount = dto.TotalAmount,
						PaymentMethodId = 1,
						Notes = "this PaymentDate is added by User",
						UserId = userId,
						SupplierId = dto.SupplierId,
					});

				return Ok("Payment_date created Succefuly");

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		// Update
		[HttpPut("{supplierPaymentDateId}")]
		public async Task<IActionResult> UpdateSupplierPaymentDate(SupplierPaymentDateBaseDto dto, int supplierPaymentDateId)
		{
			try
			{
				var existSupplierPaymentDate = await _context.SupplierPaymentDates.FindAsync(supplierPaymentDateId);

				if (existSupplierPaymentDate == null)
				{
					return BadRequest($"This supplier_payment_date = {supplierPaymentDateId} is not exist ");
				}


				await _supplierPaymentDateService.UpdateDateOfBaymentAsync(supplierPaymentDateId, dto.DateOfPayment);

				return Ok("date of payment_date updated Succefuly");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}

		// Get
		[HttpGet("{supplierPaymentDateId}")]
		public async Task<IActionResult> GetSupplierPaymentDateById(int supplierPaymentDateId)
		{
			try
			{

				var existSupplierPaymentDate = await _context.SupplierPaymentDates.FindAsync(supplierPaymentDateId);

				if (existSupplierPaymentDate == null)
				{
					return BadRequest($"This supplier_payment_date = {supplierPaymentDateId} is not exist ");
				}

				return Ok(existSupplierPaymentDate);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		[HttpGet("supplierId/{supplierId}")]
		public async Task<IActionResult> GetSupplierPaymentDateByUserId(int supplierId)
		{
			try
			{
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var existClientPaymentDate = await _context.SupplierPaymentDates.FirstOrDefaultAsync((c) => c.SupplierId == supplierId && c.UserId == userId);

				if (existClientPaymentDate == null)
				{
					return BadRequest($"This supplier_payment_date supplierId = {supplierId} is not exist ");
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
		public async Task<IActionResult> DeleteSupplierPaymentDateByUserId(int supplierPaymentDateId)
		{
			try
			{
				var existSupplierPaymentDate = await _context.SupplierPaymentDates.FindAsync(supplierPaymentDateId);

				if (existSupplierPaymentDate == null)
				{
					return BadRequest($"This supplier_payment_date = {supplierPaymentDateId} is not exist ");
				}

				_context.SupplierPaymentDates.Remove(existSupplierPaymentDate);
				await _context.SaveChangesAsync();

				return Ok(" supplier_payment_date deleted Succefuly");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

			}
		}

	}
}
