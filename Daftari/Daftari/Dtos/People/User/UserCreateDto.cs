using Daftari.Dtos.People.Person;

namespace Daftari.Dtos.People.User
{
    public class UserCreateDto : PersonCreateDto
    {
        public string StoreName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string? UserType { get; set; } // not required
		public byte SectorId { get; set; }
        public byte BusinessTypeId { get; set; }
    }
}
