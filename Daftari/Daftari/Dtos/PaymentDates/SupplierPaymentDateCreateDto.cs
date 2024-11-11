namespace Daftari.Dtos.PaymentDates
{
	public class SupplierPaymentDateCreateDto
	{
		public DateTime DateOfPayment { get; set; }

		public decimal TotalAmount { get; set; }

		public byte PaymentMethodId { get; set; }

		public string? Notes { get; set; }

		public int UserId { get; set; }

		public int SupplierId { get; set; }
	}
}
