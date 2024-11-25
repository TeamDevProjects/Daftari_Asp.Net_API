using System.Text;
using System.Security.Cryptography;

namespace Daftari.Services.HelperServices
{
    public static class PasswordHelper
    {
        public static string HashingPassword(string Password)
        {
            using (SHA256 sHA256 = SHA256.Create())
            {

                byte[] hashBytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashingPassword(password) == hashedPassword;
        }

    }
}
