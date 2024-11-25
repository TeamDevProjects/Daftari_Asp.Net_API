using Daftari.Dtos.Transactions.BasesDtos;

namespace Daftari.Dtos.Transactions.SupplierTransactionDtos
{
    public class SupplierTransactionCreateDto : TransactionDto
    {
        public byte TransactionTypeId { get; set; } // 1 => Payment , 2 => Withdrawal

        public int SupplierId { get; set; }
    }
}
