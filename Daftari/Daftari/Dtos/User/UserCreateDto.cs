namespace Daftari.Dtos.User
{
    public class UserCreateDto
    {
        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string StoreName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
        public string UserType { get; set; } = null!;

        public byte SectorId { get; set; }
        public byte BusinessTypeId { get; set; }
    }
}
