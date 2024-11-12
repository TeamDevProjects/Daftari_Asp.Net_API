using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Daftari.Helper;
using Daftari.Services.Images;
using Daftari.Services.PaymentDateServices;
using Daftari.Services.TotalAmountServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ClientTransactionsController : BaseTransactionsController
	{
		private readonly ClientPaymentDateService _clientPaymentDateService;
		private readonly ClientTotalAmountService _clientTotalAmountService;

		public ClientTransactionsController(
			DaftariContext context, JwtHelper jwtHelper,
			ClientPaymentDateService clientPaymentDateService, ClientTotalAmountService clientTotalAmountService)
			: base(context, jwtHelper)
		{
			_clientPaymentDateService = clientPaymentDateService;
			_clientTotalAmountService = clientTotalAmountService;
		}


		// Add     +
		// Update  
		// Delete  
		// Get => view
		

		[HttpPost]
		public async Task<IActionResult> CreateClientTransaction([FromForm] ClientTransactionCreateDto clientTransactionData)
		{
			// Handel Uploading Image
			var ImageObj = await ImageServices.HandelImageServices(clientTransactionData.FormImage!);

			clientTransactionData.ImageData = ImageObj.ImageData;
			clientTransactionData.ImageType = ImageObj.ImageType;

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

				// => HandleTotalAmount
				var totalAmount = await _clientTotalAmountService.SaveClientTotalAmount(
					clientTransactionData.TransactionTypeId, clientTransactionData.Amount, clientTransactionData.ClientId, userId);
				
				// craete Client Transaction
				var newClientTransactionObj = new ClientTransaction
				{
					TransactionId = newTransactionObj!.TransactionId,
					UserId = userId,
					ClientId = clientTransactionData.ClientId,
					TotalAmount= totalAmount

				};

				_context.ClientTransactions.Add(newClientTransactionObj);
				await _context.SaveChangesAsync();


				// Default PaymentDate add in firsy time or just update totalAmount
				await _clientPaymentDateService.SaveClientPaymentDate(clientTransactionData.ClientId, totalAmount,userId);


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
