using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Daftari.Helper;
using Daftari.Services.Images;
using Daftari.Services.PaymentDateServices;
using Daftari.Services.TotalAmountServices;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class SupplierTransactionsController : BaseTransactionsController
	{
		private readonly SupplierPaymentDateService _supplierPaymentDateService;
		private readonly SupplierTotalAmountService _supplierTotalAmountService;

		public SupplierTransactionsController(
			DaftariContext context, JwtHelper jwtHelper, 
			SupplierPaymentDateService supplierPaymentDateService, SupplierTotalAmountService supplierTotalAmountService)
			: base(context, jwtHelper)
		{
			_supplierPaymentDateService = supplierPaymentDateService;
			_supplierTotalAmountService = supplierTotalAmountService;
		}


		// Add     +
		// Update  
		// Delete  
		// Get


		[HttpPost]
		public async Task<IActionResult> CreateSupplierTransaction([FromForm] SupplierTransactionCreateDto SupplierTransactionData)
		{
			// Handel Uploading Image
			var ImageObj = await ImageServices.HandelImageServices(SupplierTransactionData.FormImage!);

			SupplierTransactionData.ImageData = ImageObj.ImageData;
			SupplierTransactionData.ImageType = ImageObj.ImageType;

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
						TransactionTypeId = SupplierTransactionData.TransactionTypeId,
						Notes = SupplierTransactionData.Notes,
						TransactionDate = DateTime.Now,
						Amount = SupplierTransactionData.Amount,
						ImageData = SupplierTransactionData.ImageData,
						ImageType = SupplierTransactionData.ImageType,

					});

				if (newTransactionObj == null)
				{
					return BadRequest("can not add Transaction");
				}

				// => HandleTotalAmount
				var totalAmount = await _supplierTotalAmountService
					.SaveSupplierTotalAmount(SupplierTransactionData.TransactionTypeId, SupplierTransactionData.Amount, SupplierTransactionData.SupplierId, userId);

				// craete Client Transaction
				var newSupplierTransactionObj = new SupplierTransaction
				{
					TransactionId = newTransactionObj!.TransactionId,
					UserId = userId,
					SupplierId = SupplierTransactionData.SupplierId,
					TotalAmount = totalAmount

				};

				 _context.SupplierTransactions.Add(newSupplierTransactionObj);
				await _context.SaveChangesAsync();


				// Default PaymentDate add in firsy time or just update totalAmount
				await _supplierPaymentDateService.SaveSupplierPaymentDate(SupplierTransactionData.SupplierId, totalAmount,userId);

				await transaction.CommitAsync();
				return Ok("Supplier Transaction Created Succfuly");
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
