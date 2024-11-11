using Daftari.Data;
using Daftari.Dtos.PaymentDates;
using Daftari.Entities;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace Daftari.Services.PaymentDateServices
{
	public class ClientPaymentDateService:PaymentDateService
	{

		public ClientPaymentDateService(DaftariContext context):base(context){}

		public async Task<ClientPaymentDate> CreateClientPaymentDateAsync(ClientPaymentDateCreateDto clientPaymentDateData)
		{
			if (clientPaymentDateData == null)
			{
				throw new ArgumentNullException(nameof(clientPaymentDateData));
			}

			try
			{

				// add PaymentDate
				var paymentDate = await CreatePaymentDateService(new PaymentDate
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
