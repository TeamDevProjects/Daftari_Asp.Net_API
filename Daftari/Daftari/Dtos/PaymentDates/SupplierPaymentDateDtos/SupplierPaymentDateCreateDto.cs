using Daftari.Dtos.PaymentDates.Bases;

namespace Daftari.Dtos.PaymentDates.SupplierPaymentDateDtos
{
    public class SupplierPaymentDateCreateDto: SupplierPaymentDateBaseDto
	{
        public int UserId { get; set; }


		public decimal TotalAmount { get; set; }  

		public byte PaymentMethodId { get; set; } 

	}
}
