using Daftari.Dtos.Transactions.BasesDtos;

namespace Daftari.Dtos.Transactions.ClientTransactionDto
{
    public class ClientTransactionCreateDto : TransactionDto
    {
		public byte TransactionTypeId { get; set; } // 1 => Payment , 2 => Withdrawal

		public int ClientId { get; set; }

    }
}
