using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(DaftariContext context) : base(context) { }

		public async Task<IEnumerable<UsersView>> GetAll()
		{
			try
			{
				var users = await _context.UsersViews.ToListAsync();

				if (users.Any()) return users;

				return null;
			}
			catch (Exception) { return null; }
		}

		public async Task<UsersView> GetUserView(int userId)
		{
			try
			{
				var user = await _context.UsersViews.FirstOrDefaultAsync((u)=> u.UserId == userId);

				if (user == null) return null;

				return user;
			}
			catch (Exception) { return null; }
		}


		public async Task<IEnumerable<UsersView>> SearchByName(string temp)
		{
			try
			{
				var users = await _context.UsersViews.Where((u) => u.Name.Contains(temp)).ToListAsync();

				if (users.Any()) return users;

				return null;
			}
			catch (Exception) { return null; }
		}
	}
}
