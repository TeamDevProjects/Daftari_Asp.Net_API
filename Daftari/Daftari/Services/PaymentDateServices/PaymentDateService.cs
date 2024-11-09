using Daftari.Data;
using Daftari.Entities;

namespace Daftari.Services.PaymentDateServices
{
	public class PaymentDateService
	{
		protected readonly DaftariContext _context;

		public PaymentDateService(DaftariContext context)
		{
			_context = context;
		}

		protected async Task<PaymentDate> CreatePaymentDateService(PaymentDate service)
		{
			try
			{

				// add PaymentDate
				var paymentDate = new PaymentDate
				{
					PaymentDate1 = service.PaymentDate1,
					TotalAmount = service.TotalAmount,  // calcu
					PaymentMethodId = service.PaymentMethodId,
					Notes = service.Notes,
				};

				await _context.PaymentDates.AddAsync(paymentDate);

				return paymentDate;
			}
			catch (Exception ex) 
			{
				throw ex;

			}

		}
	}
}
