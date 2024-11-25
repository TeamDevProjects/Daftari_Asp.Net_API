using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class PersonRepository : Repository<Person>, IPersonRepository
	{
		public PersonRepository(DaftariContext context):base(context)
		{
		}
		public async Task<bool> CheckPhoneIsExistAsync(string Phone)
		{
			// provide douplicate client phone
			if (await _context.People.AnyAsync(u => u.Phone == Phone))
				return true;

			return false;
		}


	}


}
