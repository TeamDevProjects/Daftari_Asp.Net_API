namespace Daftari.Dtos.People.User
{
    public class UserLoginDto
    {
        public string UserName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

    }
}
