using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Services.IServices;

namespace Daftari.Services
{
    public class PaymentDateService: IPaymentDateService
	{
        private readonly IPaymentDateRepository _paymentDateRepository;
        public PaymentDateService(IPaymentDateRepository paymentDateRepositpry)
        {
            _paymentDateRepository = paymentDateRepositpry;
        }

        public async Task<PaymentDate> UpdateDatePaymentDateAsync(int paymentDateId, DateTime dateOfPayment, string notes)
        {

            var existPaymentDate = await _paymentDateRepository.GetByIdAsync(paymentDateId);

            if (existPaymentDate == null)
            {
                throw new KeyNotFoundException($"paymentDateId {paymentDateId} not found");
            }

            existPaymentDate.DateOfPayment = dateOfPayment;
            existPaymentDate.Notes = notes;

            var isUpdated = await _paymentDateRepository.UpdateAsync(existPaymentDate);

            if (!isUpdated) throw new InvalidOperationException("Unable to update DateOfPaymentDate");

            return existPaymentDate;
        }
    }
}
