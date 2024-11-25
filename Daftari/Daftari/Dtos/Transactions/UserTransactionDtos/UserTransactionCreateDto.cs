using Daftari.Dtos.Transactions.BasesDtos;

namespace Daftari.Dtos.Transactions.UserTransactionDtos
{
    public class UserTransactionCreateDto : TransactionDto
    {
        public byte TransactionTypeId { get; set; } // 1 => Payment , 2 => Withdrawal

    }
}
