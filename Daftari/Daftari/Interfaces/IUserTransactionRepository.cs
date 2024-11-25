using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface IUserTransactionRepository:IRepository<UserTransaction>
	{
		Task<IEnumerable<UserTransaction>> GetAllAsync(int userId);
	}
}
