using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface IUserTotalAmountRepository:IRepository<UserTotalAmount>
	{
		Task<UserTotalAmount> GetByUserIdAsync(int userId);
	}
}
