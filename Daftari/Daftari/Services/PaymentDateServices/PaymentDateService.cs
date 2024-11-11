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
					DateOfPayment = service.DateOfPayment,
					TotalAmount = service.TotalAmount,  // calcu
					PaymentMethodId = service.PaymentMethodId,
					Notes = service.Notes,
				};

				await _context.PaymentDates.AddAsync(paymentDate);
				await _context.SaveChangesAsync();

				return paymentDate;
			}
			catch (Exception ex) 
			{
				throw ex;

			}

		}
		public async Task<PaymentDate> UpdatePaymentDateTotalAmountAsync(int paymentDateId, decimal totalAmount)
		{
			var existPaymentDate = await _context.PaymentDates.FindAsync(paymentDateId);

			if (existPaymentDate == null) 
			{
				throw new Exception($"paymentDateId {paymentDateId} not found");
			}

			existPaymentDate!.TotalAmount = totalAmount;	
			await _context.SaveChangesAsync();

			return existPaymentDate;
		}
	}
}
