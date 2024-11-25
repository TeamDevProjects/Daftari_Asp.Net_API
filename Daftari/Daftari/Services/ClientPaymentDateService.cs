using Daftari.Data;
using Daftari.Dtos.PaymentDates.ClientPaymentDateDtos;
using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Repositories;
using Daftari.Services.HelperServices;
using Daftari.Services.IServices;
using Microsoft.EntityFrameworkCore;

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
                    DateOfPayment = DateTime.UtcNow.AddDays(20),
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

    }
}
