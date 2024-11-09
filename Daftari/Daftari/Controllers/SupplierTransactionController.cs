using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.People.Person;
using Daftari.Dtos.People.Supplier;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Daftari.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class SupplierTransactionController : BaseTransactionController
	{
		public SupplierTransactionController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
		{

		}


		// Add     +
		// Update  
		// Delete  
		// Get



		[HttpPost]
		public async Task<IActionResult> CreateTransaction([FromForm] SupplierTransactionCreateDto supplierTransactionData)
		{
			if (supplierTransactionData.FormImage != null)
			{
				try
				{
					long fileSizeLimit = 10 * 1024 * 1024; // 10 MB size limit
					if (supplierTransactionData.FormImage.Length > fileSizeLimit)
					{
						return BadRequest("File size exceeds the allowed limit.");
					}

					using (var memoryStream = new MemoryStream())
					{
						await supplierTransactionData.FormImage.CopyToAsync(memoryStream);
						supplierTransactionData.ImageData = memoryStream.ToArray();  // Convert to byte array
						supplierTransactionData.ImageType = supplierTransactionData.FormImage.ContentType;  // Set MediaType from the file
					}
				}
				catch (Exception ex)
				{
					// Log the exception (e.g., using a logging framework)
					return BadRequest($"File upload failed: {ex.Message}");
				}
			}

			// Check if MediaData is still null and set MediaType to "None"
			if (supplierTransactionData.ImageData == null) supplierTransactionData.ImageType = "None";

			// Validate MediaType if necessary
			var validMediaTypes = new[] { "image/jpeg", "image/png", "None" };
			if (!validMediaTypes.Contains(supplierTransactionData.ImageType))
			{
				return BadRequest("Invalid media type. Only specific file types are allowed.");
			}

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
				var newTransactionObj = await CreateTransactionAsync(new Transaction
				{
					TransactionTypeId = supplierTransactionData.TransactionTypeId,
					Notes = supplierTransactionData.Notes,
					TransactionDate = DateTime.Now,
					Amount = supplierTransactionData.Amount,
					ImageData = supplierTransactionData.ImageData!,
					ImageType = supplierTransactionData.ImageType,
				});

				if (newTransactionObj != null)
				{
					return BadRequest("can not add Supplier Transaction");
				}

				var newSupplierTransactionObj = new SupplierTransaction
				{
					TransactionId = newTransactionObj!.TransactionId,
					UserId = userId,
					SupplierId = supplierTransactionData.SupplierId,
					TotalAmount = supplierTransactionData.Amount

				};

				_context.SupplierTransactions.Add(newSupplierTransactionObj);
				await _context.SaveChangesAsync();

				// try get isexist null => add, true => update
				// Add/Update Default ClientPaymentDate 
				//  dont update the date (dont add it)



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
