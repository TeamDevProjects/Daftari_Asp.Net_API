using Daftari.Dtos.People.User;
using Daftari.Entities.Views;
using Daftari.Services.HelperServices;

namespace Daftari.Services.InterfacesServices
{
	public interface IUserService
	{
		Task<bool> AddUserAsync(UserCreateDto userData);
		Task<bool> UpdateUserAsync(UserUpdateDto userData, int UserId);
		Task<bool> DeleteUserAsync(int UserId);
		Task<UsersView> GetUserView(int userId);
		Task<IEnumerable<UsersView>> GetAll();
		Task<IEnumerable<UsersView>> SearchForUsersByName(string temp);

	}
}
