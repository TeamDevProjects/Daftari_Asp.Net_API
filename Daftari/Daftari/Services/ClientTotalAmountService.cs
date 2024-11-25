using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services
{
    public class ClientTotalAmountService: IClientTotalAmountService
	{
        private IClientTotalAmountRepository _clientTotalAmountRepository;

        public ClientTotalAmountService(IClientTotalAmountRepository clientTotalAmountRepository)
        {
            _clientTotalAmountRepository = clientTotalAmountRepository;
        }

        public async Task<decimal> AddClientTotalAmountAsync(decimal totalAmount, int userId, int clientId)
        {
            var newClientTotalAmount = new ClientTotalAmount
            {
                UpdateAt = DateTime.UtcNow,
                UserId = userId,
                ClientId = clientId,
                TotalAmount = totalAmount
            };

            var ClientTotalAmountAdded = await _clientTotalAmountRepository.AddAsync(newClientTotalAmount);

            if (ClientTotalAmountAdded == null) throw new InvalidOperationException("Unable to add client total amount");

            return ClientTotalAmountAdded.TotalAmount;

        }

        public async Task<decimal> UpdateClientTotalAmountAsync(ClientTotalAmount existClientTotalAmount, decimal Amount, byte TransactionTypeId)
        {

            decimal totalAmount = Amount;

            if (TransactionTypeId == 1)
            {
                totalAmount = existClientTotalAmount.TotalAmount - Amount;

                existClientTotalAmount.TotalAmount = totalAmount;
            }
            else if (TransactionTypeId == 2)
            {
                totalAmount = existClientTotalAmount.TotalAmount + Amount;

                existClientTotalAmount.TotalAmount = totalAmount;
            }

            existClientTotalAmount.UpdateAt = DateTime.UtcNow; // Update timestamp

            var isUpdated = await _clientTotalAmountRepository.UpdateAsync(existClientTotalAmount);

            if (!isUpdated) throw new InvalidOperationException("Unable to update client total amount");

            return totalAmount;
        }

        public async Task<decimal> SaveClientTotalAmountAsync(byte TransactionTypeId, decimal Amount, int clientId, int userId)
        {
            decimal totalAmount = Amount;

            var existUserTotalAmount = await _clientTotalAmountRepository.GetByClientIdAsync(clientId);

            if (existUserTotalAmount == null)
            {
                var clientTotalAmountAdded = await AddClientTotalAmountAsync(totalAmount, userId, clientId);

                totalAmount = clientTotalAmountAdded;
            }
            else
            {
                totalAmount = await UpdateClientTotalAmountAsync(existUserTotalAmount, Amount, TransactionTypeId);
            }

            return totalAmount;
        }

        public async Task<ClientTotalAmount> GetClientTotalAmountByClientId(int clientId)
        {
            var clientTotalAmount = await _clientTotalAmountRepository.GetByClientIdAsync(clientId);
            if (clientTotalAmount == null) throw new KeyNotFoundException("clientTotalAmount not found");

            return clientTotalAmount;
        }

    }
}
