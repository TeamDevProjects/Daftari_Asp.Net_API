using System.ComponentModel.DataAnnotations;

namespace Daftari.Dtos.PaymentDates.Bases
{
	public class PaymentDateBaseDto
	{
		[Required]
		public DateTime DateOfPayment { get; set; }

		public string? Notes { get; set; }
	}
}
