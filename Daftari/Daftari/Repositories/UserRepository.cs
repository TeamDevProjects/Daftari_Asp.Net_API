using Daftari.Data;
using Daftari.Dtos.People.User;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(DaftariContext context) : base(context) { }

	}
}
