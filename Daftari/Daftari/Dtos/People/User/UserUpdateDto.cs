using Daftari.Dtos.People.Person;

namespace Daftari.Dtos.People.User
{
	public class UserUpdateDto:PersonUpdateDto
	{
		public string StoreName { get; set; } = null!;

		public string UserName { get; set; } = null!;

		public string PasswordHash { get; set; } = null!;

		//public byte SectorId { get; set; }

		//public byte BusinessTypeId { get; set; }
	}
}
