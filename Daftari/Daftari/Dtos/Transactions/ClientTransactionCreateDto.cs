namespace Daftari.Dtos.Transactions
{
    public class ClientTransactionCreateDto : TransactionCreateDto
    {
        //public int UserId { get; set; }

        public int ClientId { get; set; }

        //public decimal TotalAmount { get; set; } = 0; must be = Amount
    }
}
