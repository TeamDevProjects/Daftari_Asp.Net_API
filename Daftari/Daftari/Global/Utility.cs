using System.Text;
using System.Security.Cryptography;

namespace Daftari.Global
{
	public static class Utility
	{
		public static string HashingPassword(string Password)
		{
			using (SHA256 sHA256 = SHA256.Create())
			{

				byte[] hashBytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Password));
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}

	}
}
