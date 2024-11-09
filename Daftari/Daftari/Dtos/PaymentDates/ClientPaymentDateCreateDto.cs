namespace Daftari.Dtos.PaymentDates
{
	public class ClientPaymentDateCreateDto
	{
		public DateTime PaymentDate1 { get; set; }

		public decimal TotalAmount { get; set; }

		public byte PaymentMethodId { get; set; } 

		public string? Notes { get; set; }

		public int UserId { get; set; }

		public int ClientId { get; set; }

	}
}
