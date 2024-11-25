using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;

namespace Daftari.Repositories
{
	public class TransactionRepository : Repository<Transaction>, ITransactionRepository
	{
		public TransactionRepository(DaftariContext context) : base(context) { }


	}
}
