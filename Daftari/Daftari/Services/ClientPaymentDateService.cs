using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Services.IServices;
using Daftari.Dtos.PaymentDates.ClientPaymentDateDtos;
using Daftari.Entities.Views;
using Daftari.Repositories;

namespace Daftari.Services
{
    public class ClientPaymentDateService: IClientPaymentDateService
	{
        private readonly IPaymentDateRepository _paymentDateRepository;
        private readonly IClientPaymentDateRepository _clientpaymentDateRepository;
        public ClientPaymentDateService(IPaymentDateRepository paymentDateRepository, IClientPaymentDateRepository clientpaymentDateRepository)
        {
            _paymentDateRepository = paymentDateRepository;
            _clientpaymentDateRepository = clientpaymentDateRepository;
        }


        public async Task<ClientPaymentDate> AddClientPaymentDateAsync(ClientPaymentDateCreateDto clientPaymentDateData)
        {

            var newPayment = new PaymentDate
            {
                DateOfPayment = clientPaymentDateData.DateOfPayment,
                PaymentMethodId = clientPaymentDateData.PaymentMethodId,
                Notes = clientPaymentDateData.Notes,
            };
            // Add PaymentDate and ensure it is saved
            var paymentDate = await _paymentDateRepository.AddAsync(newPayment);

            if (paymentDate == null) throw new InvalidOperationException("PaymentDate did not save successfully.");

            // Add ClientPaymentDates
            var clientPaymentDate = new ClientPaymentDate
            {
                PaymentDateId = paymentDate.PaymentDateId,
                UserId = clientPaymentDateData.UserId,
                ClientId = clientPaymentDateData.ClientId,
            };

            var clientPaymentDateAdded = await _clientpaymentDateRepository.AddAsync(clientPaymentDate);

            if (clientPaymentDateAdded == null) throw new InvalidOperationException("ClientPaymentDate did not save successfully.");


            return clientPaymentDateAdded;

        }

        public async Task<ClientPaymentDate> SaveClientPaymentDateAsync(int clientId, decimal totalAmount, int userId)
        {
            var existClientPaymentDate = await _clientpaymentDateRepository.GetByClientIdAsync(clientId);

            if (existClientPaymentDate == null)
            {
                var newClientPaymentDate = new ClientPaymentDateCreateDto
                {
                    DateOfPayment = DateTime.UtcNow.AddDays(30),
                    TotalAmount = totalAmount,
                    PaymentMethodId = 1,
                    Notes = "this PaymentDate is added by default after 20 days from the first transaction",
                    UserId = userId,
                    ClientId = clientId
                };

                return await AddClientPaymentDateAsync(newClientPaymentDate);
            }
            else
            {
                var paymentDate = await _paymentDateRepository.GetByIdAsync(existClientPaymentDate.PaymentDateId);
				
                if (paymentDate .DateOfPayment < DateTime.Today) return existClientPaymentDate;

				// if dateOfPayment was expired update it 
				paymentDate.DateOfPayment = DateTime.UtcNow.AddDays(30);

                await _paymentDateRepository.UpdateAsync(paymentDate);

                return existClientPaymentDate;

			}

        }

        public async Task<ClientPaymentDate> GetClientPaymentDateByIdAsync(int clientPaymentDateId)
        {
            var clientPaymentDate = await _clientpaymentDateRepository.GetByIdAsync(clientPaymentDateId);

            if (clientPaymentDate == null)
            {
                throw new KeyNotFoundException($"clientPaymentDateId = {clientPaymentDateId} is found ");
            }

            return clientPaymentDate;

        }
        public async Task<ClientPaymentDate> GetClientPaymentDateByClientIdAsync(int clientId)
        {
            var clientPaymentDate = await _clientpaymentDateRepository.GetByClientIdAsync(clientId);

            if (clientPaymentDate == null)
            {
                throw new KeyNotFoundException($"clientPaymentDateId = {clientId} is found ");
            }

            return clientPaymentDate;

        }

        public async Task<bool> DeleteClientPaymentDateAsync(int clientPaymentDateId)
        {

            var clientpaymentDate = await _clientpaymentDateRepository.GetByIdAsync(clientPaymentDateId);

            if (clientpaymentDate == null) throw new KeyNotFoundException($"This payment_date = {clientPaymentDateId} is not exist ");

            var isCPaymentDateDeleted = await _clientpaymentDateRepository.DeleteAsync(clientPaymentDateId);

            var isPaymentDateDeleted = await _paymentDateRepository.DeleteAsync(clientpaymentDate.PaymentDateId);

            if (!isCPaymentDateDeleted && !isPaymentDateDeleted) throw new InvalidOperationException("Unable to delete client paymentdate");

            return true;
        }

        public async Task<bool> DeleteClientPaymentDateByClientIdAsync(int clientId)
        {

            var clientpaymentDate = await _clientpaymentDateRepository.GetByClientIdAsync(clientId);

            if (clientpaymentDate == null) throw new KeyNotFoundException($"This payment_date = {clientId} is not exist ");


            var isCPaymentDateDeleted = await _clientpaymentDateRepository.DeleteAsync(clientId);

            var isPaymentDateDeleted = await _paymentDateRepository.DeleteAsync(clientpaymentDate.PaymentDateId);


            if (!isCPaymentDateDeleted && !isPaymentDateDeleted) throw new InvalidOperationException("Unable to delete client paymentdate");

            return true;
        }


		// Get View 
		public async Task<ClientsPaymentDateView> GetPaymentDateViewByClientAsync(int ClientId)
		{

			var ClientPaymentDate = await _clientpaymentDateRepository.GetPaymentDateViewAsync(ClientId);

			if (ClientPaymentDate == null)
			{
				throw new KeyNotFoundException($"ClientId = {ClientId} is not found ");
			}

			return ClientPaymentDate;
		}
		// Get All Closer
		public async Task<IEnumerable<ClientsPaymentDateView>> GetAllCloserPaymentsDateAsync(int userId)
		{

			var ClientPaymentDate = await _clientpaymentDateRepository.GetAllCloserPaymentsDateViewAsync(userId);

			if (ClientPaymentDate == null)
			{
				throw new KeyNotFoundException($"no paymentDate founded");
			}

			return ClientPaymentDate;
		}
		// Get All Older
		public async Task<IEnumerable<ClientsPaymentDateView>> GetAllOldPaymentsDateAsync(int userId)
		{

			var ClientPaymentDate = await _clientpaymentDateRepository.GetAllOldPaymentsDateViewAsync(userId);

			if (ClientPaymentDate == null)
			{
				throw new KeyNotFoundException($"no paymentDate founded");
			}

			return ClientPaymentDate;
		}
		// Get All Today
		public async Task<IEnumerable<ClientsPaymentDateView>> GetAllToDayPaymentsDateAsync(int userId)
		{

			var ClientPaymentDate = await _clientpaymentDateRepository.GetAllToDayPaymentsDateViewAsync(userId);

			if (ClientPaymentDate == null)
			{
				throw new KeyNotFoundException($"no paymentDate founded");
			}

			return ClientPaymentDate;
		}
	}
}
