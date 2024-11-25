using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class UserTotalAmountRepository : Repository<UserTotalAmount>, IUserTotalAmountRepository
	{
		public UserTotalAmountRepository(DaftariContext context) : base(context) { }
		public async Task<UserTotalAmount> GetByUserIdAsync(int userId)
		{
			return await _context.UserTotalAmounts.FirstOrDefaultAsync(x => x.UserId == userId);
		}
	}
}
