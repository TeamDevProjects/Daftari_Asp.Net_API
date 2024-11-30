using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface IUserTransactionRepository:IRepository<UserTransaction>
	{
		Task<IEnumerable<UserTransactionsView>> GetAllAsync(int userId);
	}
}
