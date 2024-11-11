namespace Daftari.Entities
{
	public class ClientTotalAmount
	{
		public int ClientTotalAmountId { get; set; }
		public int ClientId { get; set; }  // Make sure these property names match your DB column names
		public int UserId { get; set; }
		public decimal TotalAmount { get; set; }
		public DateTime UpdateAt { get; set; }

		public Client Client { get; set; }  // Navigation property
		public User User { get; set; }      // Navigation property

	}
}
