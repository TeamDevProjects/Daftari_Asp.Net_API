using Daftari.Data;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly DaftariContext _context;
		protected readonly DbSet<T> _dbSet;

		public Repository(DaftariContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public async Task<T> AddAsync(T entity)
		{
			try
			{
				var newEntity =await _dbSet.AddAsync(entity);
				await _context.SaveChangesAsync();
				return newEntity.Entity;
			}
			catch
			{
				// Log exception or handle error if needed
				return null!;
			}
		}

		public async Task<bool> UpdateAsync(T entity)
		{
			try
			{
				_dbSet.Update(entity);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				// Log exception or handle error if needed
				return false;
			}
		}

		public async Task<bool> DeleteAsync(int id)
		{
			try
			{
				var entity = await _dbSet.FindAsync(id);
				if (entity != null)
				{
					_dbSet.Remove(entity);
					await _context.SaveChangesAsync();
					return true;
				}
				return false; // Return false if entity not found
			}
			catch
			{
				// Log exception or handle error if needed
				return false;
			}
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id); 
		}


	}
}
