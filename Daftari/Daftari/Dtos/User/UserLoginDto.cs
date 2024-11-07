namespace Daftari.Dtos.User
{
    public class UserLoginDto
    {
        public string UserName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

    }
}
