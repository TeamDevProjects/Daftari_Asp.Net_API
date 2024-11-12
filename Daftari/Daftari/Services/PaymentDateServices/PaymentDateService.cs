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

		public async Task<PaymentDate> CreatePaymentDate(PaymentDate paymentDate)
		{
			try
			{

				// add PaymentDate
				var newPaymentDate = new PaymentDate
				{
					DateOfPayment = paymentDate.DateOfPayment,
					TotalAmount = paymentDate.TotalAmount,  // calcu
					PaymentMethodId = paymentDate.PaymentMethodId,
					Notes = paymentDate.Notes,
				};

				await _context.PaymentDates.AddAsync(newPaymentDate);
				await _context.SaveChangesAsync();

				return paymentDate;
			}
			catch (Exception ex) 
			{
				throw ex;

			}

		}
		public async Task<PaymentDate> UpdateTotalAmountAsync(int paymentDateId, decimal totalAmount)
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

		public async Task<PaymentDate> UpdateDateOfBaymentAsync(int paymentDateId, DateTime DateOfPayment)
		{
			var existPaymentDate = await _context.PaymentDates.FindAsync(paymentDateId);

			if (existPaymentDate == null)
			{
				throw new Exception($"paymentDateId {paymentDateId} not found");
			}

			existPaymentDate!.DateOfPayment = DateOfPayment;
			await _context.SaveChangesAsync();

			return existPaymentDate;
		}
	}
}
