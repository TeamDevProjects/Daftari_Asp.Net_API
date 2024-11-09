using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.PaymentDates;
using Daftari.Dtos.People.Supplier;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Daftari.Helper;
using Daftari.Services.PaymentDateServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ClientTransactionController : BaseTransactionController
	{
		private readonly ClientPaymentDateService _clientPaymentDateService;

		public ClientTransactionController(DaftariContext context, JwtHelper jwtHelper, ClientPaymentDateService clientPaymentDateService)
			: base(context, jwtHelper)
		{
			_clientPaymentDateService = clientPaymentDateService;
		}


		// Add     +
		// Update  
		// Delete  
		// Get => view




		[HttpPost]
		public async Task<IActionResult> CreateTransaction([FromForm] ClientTransactionCreateDto clientTransactionData)
		{
			if (clientTransactionData.FormImage != null)
			{
				try
				{
					long fileSizeLimit = 10 * 1024 * 1024; // 10 MB size limit
					if (clientTransactionData.FormImage.Length > fileSizeLimit)
					{
						return BadRequest("File size exceeds the allowed limit.");
					}

					using (var memoryStream = new MemoryStream())
					{
						await clientTransactionData.FormImage.CopyToAsync(memoryStream);
						clientTransactionData.ImageData = memoryStream.ToArray();  // Convert to byte array
						clientTransactionData.ImageType = clientTransactionData.FormImage.ContentType;  // Set MediaType from the file
					}
				}
				catch (Exception ex)
				{
					// Log the exception (e.g., using a logging framework)
					return BadRequest($"File upload failed: {ex.Message}");
				}
			}

			// Check if MediaData is still null and set MediaType to "None"
			if (clientTransactionData.ImageData == null) clientTransactionData.ImageType = null;

			// Validate MediaType if necessary
			var validMediaTypes = new[] { "image/jpeg", "image/png", null };
			if (!validMediaTypes.Contains(clientTransactionData.ImageType))
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
				var newTransactionObj = await CreateTransactionAsync(
					new Transaction
				{
					TransactionTypeId = clientTransactionData.TransactionTypeId,
					Notes = clientTransactionData.Notes,
					TransactionDate = DateTime.Now,
					Amount = clientTransactionData.Amount,
					ImageData = clientTransactionData.ImageData,
					ImageType = clientTransactionData.ImageType,
					
				});

				if (newTransactionObj == null) 
				{
					return BadRequest("can not add Transaction");
				}


				// craete Client Transaction
				var newClientTransactionObj = new ClientTransaction
				{
					TransactionId = newTransactionObj!.TransactionId,
					UserId = userId,
					ClientId = clientTransactionData.ClientId,
					TotalAmount= clientTransactionData.Amount

				};

				_context.ClientTransactions.Add(newClientTransactionObj);
				await _context.SaveChangesAsync();

				// Add/Update Default ClientPaymentDate 
				
				//  dont update the date (dont add it)
				//var d = await _context.
				//decimal totalAmount = 0;
				//if (clientTransactionData.TransactionTypeId == 1)
				//{
				//	totalAmount += clientTransactionData.Amount;
				//}
				//else if (clientTransactionData.TransactionTypeId == 2)
				//{
				//	totalAmount -= clientTransactionData.Amount;

				//}
				//if (await _clientPaymentDateService.GetClientPaymentDateAsync(clientTransactionData.ClientId) == null)
				//{

				//	var p = await _clientPaymentDateService.CreateClientPaymentDateAsync(new ClientPaymentDateCreateDto
				//	{
				//		PaymentDate1 = DateTime.UtcNow.AddDays(7),
				//		TotalAmount  = 0,
				//		PaymentMethodId = 1,
				//		Notes="",
				//		UserId = 1,
				//		ClientId = 2

				//	});
				//}
				//else
				//{
				//	// update exist one
				//}



				await transaction.CommitAsync();
				return Ok("Transaction Created Succfuly");
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
