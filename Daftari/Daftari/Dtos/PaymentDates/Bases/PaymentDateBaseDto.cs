namespace Daftari.Dtos.PaymentDates.Bases
{
	public class PaymentDateBaseDto
	{
		public DateTime DateOfPayment { get; set; }

		public decimal TotalAmount { get; set; }

		public byte PaymentMethodId { get; set; }

		public string? Notes { get; set; }
	}
}
