using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.PaymentDates;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Daftari.Helper;
using Daftari.Services.Images;
using Daftari.Services.PaymentDateServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class SupplierTransactionController : BaseTransactionController
	{
		private readonly SupplierPaymentDateService _supplierPaymentDateService;

		public SupplierTransactionController(DaftariContext context, JwtHelper jwtHelper, SupplierPaymentDateService supplierPaymentDateService)
			: base(context, jwtHelper)
		{
			_supplierPaymentDateService = supplierPaymentDateService;
		}


		// Add     +
		// Update  
		// Delete  
		// Get



		private async Task<decimal> SaveSupplierTotalAmount(SupplierTransactionCreateDto SupplierTransactionData, int userId)
		{
			decimal totalAmount = SupplierTransactionData.Amount;

			try
			{
				var existSupplierTotalAmount = await _context.SupplierTotalAmounts
					.FirstOrDefaultAsync(c => c.SupplierId == SupplierTransactionData.SupplierId && c.UserId == userId);

				if (existSupplierTotalAmount == null)
				{
					totalAmount = SupplierTransactionData.Amount;
					// Create new Client TotalAmount
					var newSupplierTotalAmount = new SupplierTotalAmount
					{
						UpdateAt = DateTime.UtcNow,
						SupplierId = SupplierTransactionData.SupplierId,
						UserId = userId,
						TotalAmount = totalAmount
					};

					await _context.SupplierTotalAmounts.AddAsync(newSupplierTotalAmount);
					await _context.SaveChangesAsync();
				}
				else
				{
					if (SupplierTransactionData.TransactionTypeId == 1)
					{
						totalAmount = existSupplierTotalAmount.TotalAmount - SupplierTransactionData.Amount;

						existSupplierTotalAmount.TotalAmount = totalAmount;
					}
					else if (SupplierTransactionData.TransactionTypeId == 2)
					{
						totalAmount = existSupplierTotalAmount.TotalAmount + SupplierTransactionData.Amount;

						existSupplierTotalAmount.TotalAmount = totalAmount;
					}

					existSupplierTotalAmount.UpdateAt = DateTime.UtcNow; // Update timestamp

					_context.SupplierTotalAmounts.Update(existSupplierTotalAmount);
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

		private async Task SaveSupplierPaymentDate(int supplierId, int userId, decimal totalAmount)
		{
			try
			{
				var existSupplierPaymenttDate = await _context.SupplierPaymentDates
					.FirstOrDefaultAsync(c => c.SupplierId == supplierId && c.UserId == userId);

				if (existSupplierPaymenttDate == null)
				{

					await _supplierPaymentDateService.CreateSupplierPaymentDateAsync(
						new SupplierPaymentDateCreateDto
						{
							DateOfPayment = DateTime.UtcNow.AddDays(20),
							TotalAmount = totalAmount,
							PaymentMethodId = 1,
							Notes = "this PaymentDate is added by default after 20 days from the first transaction",
							UserId = userId,
							SupplierId = supplierId
						});
				}
				else
				{
					await _supplierPaymentDateService.UpdatePaymentDateTotalAmountAsync(existSupplierPaymenttDate.PaymentDateId, totalAmount);
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}


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
				var totalAmount = await SaveSupplierTotalAmount(SupplierTransactionData, userId);

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
				await SaveSupplierPaymentDate(SupplierTransactionData.SupplierId, userId, totalAmount);

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
