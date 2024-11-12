using Daftari.Data;
using Daftari.Dtos.PaymentDates.ClientPaymentDateDtos;
using Daftari.Entities;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace Daftari.Services.PaymentDateServices
{
    public class ClientPaymentDateService:PaymentDateService
	{

		public ClientPaymentDateService(DaftariContext context) :base(context)
		{
		}

		public async Task<ClientPaymentDate> CreateClientPaymentDateAsync(ClientPaymentDateCreateDto clientPaymentDateData)
		{
			if (clientPaymentDateData == null)
			{
				throw new ArgumentNullException(nameof(clientPaymentDateData));
			}

			try
			{

				// add PaymentDate
				var paymentDate = await CreatePaymentDate(new PaymentDate
				{
					DateOfPayment = clientPaymentDateData.DateOfPayment,
					TotalAmount = clientPaymentDateData.TotalAmount,  // calcu
					PaymentMethodId = clientPaymentDateData.PaymentMethodId,
					Notes = clientPaymentDateData.Notes,
				});


				// add ClientPaymentDates
				var clientPaymentDate = new ClientPaymentDate
				{
					PaymentDateId = paymentDate.PaymentDateId,
					UserId = clientPaymentDateData.UserId,
					ClientId = clientPaymentDateData.ClientId,

				};
				await _context.ClientPaymentDates.AddAsync(clientPaymentDate);
				await _context.SaveChangesAsync();


				return clientPaymentDate;
			}
			catch (Exception ex) 
			{
				throw ex;
			}
		}

		public async Task SaveClientPaymentDate(int clientId, decimal totalAmount, int userId)
		{
			try
			{
				var existClientPaymentDate = await _context.ClientPaymentDates
					.FirstOrDefaultAsync(c => c.ClientId == clientId && c.UserId == userId);

				if (existClientPaymentDate == null)
				{

					await CreateClientPaymentDateAsync(
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
					await UpdatePaymentDateTotalAmountAsync(existClientPaymentDate.PaymentDateId, totalAmount);
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
		public async Task<ClientPaymentDate> GetClientPaymentDateAsync(int clientId)
		{
			try
			{
				var clientPaymentDate = await _context.ClientPaymentDates.FirstOrDefaultAsync(x => x.ClientId == clientId);
			
				if(clientPaymentDate == null)
				{
					throw new ArgumentNullException(nameof(clientPaymentDate));
				}

				return clientPaymentDate;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		


	}
}
