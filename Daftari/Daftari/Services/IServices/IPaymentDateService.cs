using Daftari.Entities;

namespace Daftari.Services.IServices
{
	public interface IPaymentDateService
	{
		Task<PaymentDate> UpdateDatePaymentDateAsync(int paymentDateId, DateTime dateOfPayment, string notes);
	}
}
