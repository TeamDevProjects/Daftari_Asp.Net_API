using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.PaymentDates;
using Daftari.Dtos.Transactions;
using Daftari.Entities;
using Daftari.Helper;
using Daftari.Services.Images;
using Daftari.Services.PaymentDateServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
		
		private async Task<decimal> SaveClientTotalAmount(ClientTransactionCreateDto clientTransactionData, int userId)
		{
			decimal totalAmount = clientTransactionData.Amount;

			try
			{
				var existClientTotalAmount = await _context.ClientTotalAmounts
					.FirstOrDefaultAsync(c => c.ClientId == clientTransactionData.ClientId && c.UserId == userId);

				if (existClientTotalAmount == null)
				{
					totalAmount = clientTransactionData.Amount;
					// Create new Client TotalAmount
					var newClientTotalAmount = new ClientTotalAmount
					{
						UpdateAt = DateTime.UtcNow,
						ClientId = clientTransactionData.ClientId,
						UserId = userId,
						TotalAmount = totalAmount
					};

					await _context.ClientTotalAmounts.AddAsync(newClientTotalAmount);
					await _context.SaveChangesAsync();
				}
				else
				{
					if (clientTransactionData.TransactionTypeId == 1)
					{
						totalAmount = existClientTotalAmount.TotalAmount - clientTransactionData.Amount;

						existClientTotalAmount.TotalAmount = totalAmount;
					}
					else if (clientTransactionData.TransactionTypeId == 2)
					{
						totalAmount = existClientTotalAmount.TotalAmount + clientTransactionData.Amount;

						existClientTotalAmount.TotalAmount = totalAmount;
					}

					existClientTotalAmount.UpdateAt = DateTime.UtcNow; // Update timestamp

					_context.ClientTotalAmounts.Update(existClientTotalAmount);
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

		private async Task SaveClientPaymentDate(int clientId, int userId, decimal totalAmount)
		{
			try
			{
				var existClientPaymentDate = await _context.ClientPaymentDates
					.FirstOrDefaultAsync(c => c.ClientId == clientId && c.UserId == userId);

				if (existClientPaymentDate == null)
				{

					await _clientPaymentDateService.CreateClientPaymentDateAsync(
						new ClientPaymentDateCreateDto
					{
						DateOfPayment = DateTime.UtcNow.AddDays(20),
						TotalAmount = totalAmount,
						PaymentMethodId = 1,
						Notes = "this PaymentDate is added by default after 20 days from the first transaction",
						UserId = userId,
						ClientId = clientId
					});
				}
				else
				{
					await _clientPaymentDateService.UpdatePaymentDateTotalAmountAsync(existClientPaymentDate.PaymentDateId, totalAmount);
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
		
		
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
				var totalAmount = await SaveClientTotalAmount(clientTransactionData,userId);
				
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
				await SaveClientPaymentDate(clientTransactionData.ClientId, userId, totalAmount);


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





		// POST
		// Add/Update Default ClientPaymentDate 

		//  dont update the date (dont add it)
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



	}
}
