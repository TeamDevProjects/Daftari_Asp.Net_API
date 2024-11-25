using Daftari.Entities;
using Daftari.Services.HelperServices;

namespace Daftari.Services.IServices
{
	public interface IPersonService
	{
		Task<bool> CheckPhoneIsExistAsync(string Phone);

	}
}
