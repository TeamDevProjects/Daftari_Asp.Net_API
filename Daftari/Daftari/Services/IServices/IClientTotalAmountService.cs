using Daftari.Entities;

namespace Daftari.Services.IServices
{
	public interface IClientTotalAmountService
	{
		Task<decimal> AddClientTotalAmountAsync(decimal totalAmount, int userId, int clientId);

		Task<decimal> UpdateClientTotalAmountAsync(ClientTotalAmount existClientTotalAmount, decimal Amount, byte TransactionTypeId);

		Task<decimal> SaveClientTotalAmountAsync(byte TransactionTypeId, decimal Amount, int clientId, int userId);

		Task<ClientTotalAmount> GetClientTotalAmountByClientId(int clientId);

	}
}
