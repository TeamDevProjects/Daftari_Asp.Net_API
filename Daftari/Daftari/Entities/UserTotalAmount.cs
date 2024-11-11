namespace Daftari.Entities
{
	public class UserTotalAmount
	{

		public int UserTotalAmountId { get; set; }
		public int UserId { get; set; }
		public decimal TotalAmount { get; set; } = 0;
		public DateTime UpdateAt { get; set; } = DateTime.Now;

		public User User { get; set; } = null!; // Navigation property
	}
}
