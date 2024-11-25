﻿using Daftari.Dtos.Transactions.ClientTransactionDto;
using Daftari.Entities;
using Daftari.Services.Images;

namespace Daftari.Services.IServices
{
	public interface IClientTransactionService
	{
		Task<ClientTransaction> AddClientTransactionAsync(ClientTransactionCreateDto clientTransactionData, int userId);

		Task<bool> UpdateClientTransactionAsync(ClientTransactionUpdateDto ClientTransactionData);

		Task<ClientTransaction> GetClientTransactionAsync(int clientTransactionId);

		Task<IEnumerable<ClientTransaction>> GetClientTransactionsAsync(int userId, int clientId);

		Task<bool> DeleteClientTransactionAsync(int clientTransactionId, int userId);

	}
}