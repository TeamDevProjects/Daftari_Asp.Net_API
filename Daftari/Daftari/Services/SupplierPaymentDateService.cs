using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Services.IServices;
using Daftari.Dtos.PaymentDates.SupplierPaymentDateDtos;
using Daftari.Entities.Views;

namespace Daftari.Services
{
    public class SupplierPaymentDateService: ISupplierPaymentDateService
	{
        private readonly IPaymentDateRepository _paymentDateRepository;
        private readonly ISupplierPaymentDateRepository _supplierpaymentDateRepository;
        public SupplierPaymentDateService(IPaymentDateRepository paymentDateRepository, ISupplierPaymentDateRepository supplierPaymentDateRepository)
        {
            _paymentDateRepository = paymentDateRepository;
            _supplierpaymentDateRepository = supplierPaymentDateRepository;
        }


        public async Task<SupplierPaymentDate> AddSupplierPaymentDateAsync(SupplierPaymentDateCreateDto supplierPaymentDateCreateDto)
        {
            if (supplierPaymentDateCreateDto == null)
            {
                throw new ArgumentNullException(nameof(supplierPaymentDateCreateDto));
            }

            var newPaymentDate = new PaymentDate
            {
                DateOfPayment = supplierPaymentDateCreateDto.DateOfPayment,
                PaymentMethodId = supplierPaymentDateCreateDto.PaymentMethodId,
                Notes = supplierPaymentDateCreateDto.Notes,
            };

            // add PaymentDate
            var paymentDate = await _paymentDateRepository.AddAsync(newPaymentDate);

            if (paymentDate == null) throw new InvalidOperationException("Unable to Add payment date");

            // add SupplierPaymentDates
            var supplierPaymentDate = new SupplierPaymentDate
            {
                PaymentDateId = paymentDate.PaymentDateId,
                UserId = supplierPaymentDateCreateDto.UserId,
                SupplierId = supplierPaymentDateCreateDto.SupplierId,

            };
            var newSupplierPaymentDate = await _supplierpaymentDateRepository.AddAsync(supplierPaymentDate);

            if (newSupplierPaymentDate == null) throw new InvalidOperationException("Unable to Add supplier payment date");

            return supplierPaymentDate;

        }

        public async Task<SupplierPaymentDate> GetSupplierPaymentDateAsync(int supplierPaymentDateId)
        {
            var SupplierPaymentDate = await _supplierpaymentDateRepository.GetByIdAsync(supplierPaymentDateId);

            if (SupplierPaymentDate == null)
            {
                throw new KeyNotFoundException($"supplierPaymentDateId = {supplierPaymentDateId} is found ");
            }

            return SupplierPaymentDate;
        }

        public async Task<SupplierPaymentDate> GetSupplierPaymentDateBySupplierAsync(int supplierId)
        {

            var SupplierPaymentDate = await _supplierpaymentDateRepository.GetBySuppliertIdAsync(supplierId);

            if (SupplierPaymentDate == null)
            {
                throw new KeyNotFoundException($"supplierId = {supplierId} is found ");
            }

            return SupplierPaymentDate;
        }

        public async Task<SupplierPaymentDate> SaveSupplierPaymentDateAsync(int supplierId, decimal totalAmount, int userId)
        {

            var existSupplierPaymenttDate = await _supplierpaymentDateRepository.GetBySuppliertIdAsync(supplierId);

            if (existSupplierPaymenttDate == null)
            {

                return await AddSupplierPaymentDateAsync(
                    new SupplierPaymentDateCreateDto
                    {
                        DateOfPayment = DateTime.UtcNow.AddDays(30),
                        TotalAmount = totalAmount,
                        PaymentMethodId = 1,
                        Notes = "this PaymentDate is added by default after 30 days from the first transaction",
                        UserId = userId,
                        SupplierId = supplierId
                    });


            }
            else
            {
				var paymentDate = await _paymentDateRepository.GetByIdAsync(existSupplierPaymenttDate.PaymentDateId);

				if (paymentDate.DateOfPayment < DateTime.Today) return existSupplierPaymenttDate;

				// if dateOfPayment was expired update it 
				paymentDate.DateOfPayment = DateTime.UtcNow.AddDays(30);

				await _paymentDateRepository.UpdateAsync(paymentDate);

				return existSupplierPaymenttDate;
			}

        }

        public async Task<bool> DeleteSupplierPaymentDateAsync(int supplierPaymentDateId)
        {

            var clientpaymentDate = await _supplierpaymentDateRepository.GetByIdAsync(supplierPaymentDateId);

            if (clientpaymentDate == null) throw new KeyNotFoundException($"This payment_date = {supplierPaymentDateId} is not exist ");


            var isSPaymentDateDeleted = await _supplierpaymentDateRepository.DeleteAsync(supplierPaymentDateId);

            var isPaymentDateDeleted = await _paymentDateRepository.DeleteAsync(clientpaymentDate.PaymentDateId);


            if (!isSPaymentDateDeleted && !isPaymentDateDeleted) throw new InvalidOperationException("Unable to delete supplier paymentdate");

            return true;
        }


		// Get View 
		public async Task<SuppliersPaymentDateView> GetPaymentDateViewBySupplierAsync(int supplierId)
		{

			var SupplierPaymentDate = await _supplierpaymentDateRepository.GetPaymentDateViewAsync(supplierId);

			if (SupplierPaymentDate == null)
			{
				throw new KeyNotFoundException($"supplierId = {supplierId} is found ");
			}

			return SupplierPaymentDate;
		}
		// Get All Closer
		public async Task<IEnumerable<SuppliersPaymentDateView>> GetAllCloserPaymentsDateAsync(int userId)
		{

			var SupplierPaymentDate = await _supplierpaymentDateRepository.GetAllCloserPaymentsDateViewAsync(userId);

			if (SupplierPaymentDate == null)
			{
				throw new KeyNotFoundException($"no data founded");
			}

			return SupplierPaymentDate;
		}
		// Get All Older
		public async Task<IEnumerable<SuppliersPaymentDateView>> GetAllOldPaymentsDateAsync(int userId)
		{

			var SupplierPaymentDate = await _supplierpaymentDateRepository.GetAllOldPaymentsDateViewAsync(userId);

			if (SupplierPaymentDate == null)
			{
				throw new KeyNotFoundException($"no data founded");
			}

			return SupplierPaymentDate;
		}
		// Get All Today
		public async Task<IEnumerable<SuppliersPaymentDateView>> GetAllToDayPaymentsDateAsync(int userId)
		{

			var SupplierPaymentDate = await _supplierpaymentDateRepository.GetAllToDayPaymentsDateViewAsync(userId);

			if (SupplierPaymentDate == null)
			{
				throw new KeyNotFoundException($"no data founded");
			}

			return SupplierPaymentDate;
		}
	}
}
