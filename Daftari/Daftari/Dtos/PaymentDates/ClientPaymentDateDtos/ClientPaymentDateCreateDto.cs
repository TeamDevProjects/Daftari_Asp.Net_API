using Daftari.Dtos.PaymentDates.Bases;

namespace Daftari.Dtos.PaymentDates.ClientPaymentDateDtos
{
    public class ClientPaymentDateCreateDto : ClientPaymentDateBaseDto
    {
        public int UserId { get; set; }

		public decimal TotalAmount { get; set; }  

		public byte PaymentMethodId { get; set; } 

	}
}
