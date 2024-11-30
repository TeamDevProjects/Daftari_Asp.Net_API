using Daftari.Data;
using Daftari.Dtos.PaymentDates.Bases;
using Daftari.Dtos.PaymentDates.SupplierPaymentDateDtos;
using Daftari.Services;
using Daftari.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    [Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class SupplierPaymentDatesController : BaseController
	{
		private readonly ISupplierPaymentDateService _supplierPaymentDateService;
		private readonly IPaymentDateService _paymentDateService;
		private readonly ISupplierTotalAmountService _supplierTotalAmountService;


		public SupplierPaymentDatesController(DaftariContext context
			, ISupplierPaymentDateService supplierPaymentDateService,IPaymentDateService paymentDateService,ISupplierTotalAmountService supplierTotalAmountService) : base(context)
		{
			_supplierPaymentDateService = supplierPaymentDateService;
			_paymentDateService = paymentDateService;
			_supplierTotalAmountService = supplierTotalAmountService;

		}


		// Add
		[HttpPost]
		public async Task<IActionResult> AddSupplierPaymentDate(SupplierPaymentDateBaseDto dto)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				var userId = GetUserIdFromToken();

				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var existSupplierPaymentDate = await _supplierPaymentDateService.GetSupplierPaymentDateBySupplierAsync(dto.SupplierId);

				if (existSupplierPaymentDate != null) return BadRequest("This Supplier payment date is exist you can not add it again");

				var supplierTotalAmount = await _supplierTotalAmountService.GetSupplierTotalAmountBySupplierId(dto.SupplierId);
				
				var supplierPaymentDate = await _supplierPaymentDateService.AddSupplierPaymentDateAsync(
					new SupplierPaymentDateCreateDto
					{
						DateOfPayment = dto.DateOfPayment,// default no
						TotalAmount = supplierTotalAmount.TotalAmount,
						PaymentMethodId = 1,
						Notes = "this PaymentDate is added by User",
						UserId = userId,
						SupplierId = dto.SupplierId,
					});

				await transaction.CommitAsync();
				return Ok(supplierPaymentDate);

			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		// Update
		[HttpPut("{supplierPaymentDateId}")]
		public async Task<IActionResult> UpdateSupplierPaymentDate(SupplierPaymentDateBaseDto dto, int supplierPaymentDateId)
		{
			var transaction = _context.Database.BeginTransaction();

			try
			{
				var existSupplierPaymentDate = await _supplierPaymentDateService.GetSupplierPaymentDateAsync(supplierPaymentDateId);

				await _paymentDateService.UpdateDatePaymentDateAsync(existSupplierPaymentDate.PaymentDateId, dto.DateOfPayment,dto.Notes);
				
				await transaction.CommitAsync();
				
				return Ok("date of payment_date updated Succefuly");
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}

		// Get
		[HttpGet("{supplierPaymentDateId}")]
		public async Task<IActionResult> GetSupplierPaymentDateById(int supplierPaymentDateId)
		{
			try
			{
				var existSupplierPaymentDate = await _supplierPaymentDateService.GetSupplierPaymentDateAsync(supplierPaymentDateId);

				return Ok(existSupplierPaymentDate);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}catch (Exception ex)
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

				var existSupplierPaymentDate = await _supplierPaymentDateService.GetSupplierPaymentDateBySupplierAsync(supplierId);

				return Ok(existSupplierPaymentDate);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		[HttpGet("View/supplierId/{supplierId}")]
		public async Task<IActionResult> GetSupplierPaymentDateViewByClientId(int supplierId)
		{
			try
			{
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var existSupplierPaymentDate = await _supplierPaymentDateService.GetPaymentDateViewBySupplierAsync(supplierId);

				return Ok(existSupplierPaymentDate);
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

		[HttpGet("today")]
		public async Task<IActionResult> GetAllTodaySupplierPaymentDateByClientId()
		{
			try
			{
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var todaySupplierPaymentDates = await _supplierPaymentDateService.GetAllToDayPaymentsDateAsync(userId);

				return Ok(todaySupplierPaymentDates);
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

		[HttpGet("closer")]
		public async Task<IActionResult> GetAllCloserSupplierPaymentDateByClientId()
		{
			try
			{
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var closerSupplierPaymentDates = await _supplierPaymentDateService.GetAllCloserPaymentsDateAsync(userId);

				return Ok(closerSupplierPaymentDates);
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

		[HttpGet("old")]
		public async Task<IActionResult> GetAllOldSupplierPaymentDateByClientId()
		{
			try
			{
				var userId = GetUserIdFromToken();
				if (userId == -1)
				{
					return Unauthorized("UserId is not founded in token");
				}

				var oldSupplierPaymentDates = await _supplierPaymentDateService.GetAllOldPaymentsDateAsync(userId);

				return Ok(oldSupplierPaymentDates);
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
		[HttpDelete("{supplierPaymentDateId}")]
		public async Task<IActionResult> DeleteSupplierPaymentDateByUserId(int supplierPaymentDateId)
		{
			var transaction = _context.Database.BeginTransaction();
			try
			{
				var existSupplierPaymentDate = await _supplierPaymentDateService.GetSupplierPaymentDateAsync(supplierPaymentDateId);

				var userId = GetUserIdFromToken();

				if (userId == -1 || userId != existSupplierPaymentDate.UserId)
				{
					return Unauthorized("UserId is not founded in token");
				}

				await _supplierPaymentDateService.DeleteSupplierPaymentDateAsync(existSupplierPaymentDate.SupplierId);

				await transaction.CommitAsync();
				return Ok(" supplier_payment_date deleted Succefuly");
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

	}
}
