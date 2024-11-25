namespace Daftari.Interfaces
{
	public interface IRepository<T> 
	{
		Task<T> GetByIdAsync(int id);
		Task<T> AddAsync(T entity);      // Returns true if added successfully
		Task<bool> UpdateAsync(T entity);   // Returns true if updated successfully
		Task<bool> DeleteAsync(int id);     // Returns true if deleted successfully
		//Task<bool> ExistsAsync(int id);     // Returns true if entity with the ID exists
	}
}
