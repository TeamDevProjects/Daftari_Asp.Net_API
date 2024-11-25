using Daftari.Dtos.People.User;
using Daftari.Services.HelperServices;

namespace Daftari.Services.InterfacesServices
{
	public interface IUserService
	{
		Task<bool> AddUserAsync(UserCreateDto userData);
		Task<bool> UpdateUserAsync(UserUpdateDto userData, int UserId);
		Task<bool> DeleteUserAsync(int UserId);

	}
}
