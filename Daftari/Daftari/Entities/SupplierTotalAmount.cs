namespace Daftari.Entities
{
	public class SupplierTotalAmount
	{
		public int SupplierTotalAmountId { get; set; }
		public int SupplierId { get; set; }
		public int UserId { get; set; }
		public decimal TotalAmount { get; set; } = 0;
		public DateTime UpdateAt { get; set; } = DateTime.Now;

		public Supplier Supplier { get; set; } = null!;  // Navigation property
		public User User { get; set; } = null!; // Navigation property
	}
}
