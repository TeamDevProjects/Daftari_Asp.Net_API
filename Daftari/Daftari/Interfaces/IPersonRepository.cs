using Daftari.Entities;

namespace Daftari.Interfaces
{
	public interface IPersonRepository :IRepository<Person> 
	{
		Task<bool> CheckPhoneIsExistAsync(string Phone);
	}
}
