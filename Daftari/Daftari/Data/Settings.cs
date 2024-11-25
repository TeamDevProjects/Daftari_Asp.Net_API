namespace Daftari.Data
{
	public class clsJWT
	{
		public readonly string Key = string.Empty;
		public readonly string Issuer = string.Empty;
		public readonly string Audience = string.Empty;
		public readonly double ExpireMinutes = 0;
		public clsJWT(string key, string issuer, string audience, double expireMinutes)
		{
			Key = key;
			Issuer = issuer;
			Audience = audience;
			ExpireMinutes = expireMinutes;
		}
	}

	public class Settings
	{
		static public string? ConnectionString { get; private set; }
		static public clsJWT JWT { get; private set; }

		static Settings()
		{
			var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			ConnectionString = config.GetConnectionString("DefaultConnectionString");

			// Access Jwt settings
			var key = config["Jwt:Key"];
			var issuer = config["Jwt:Issuer"];
			var audience = config["Jwt:Audience"];
			var expireMinutes = config["Jwt:ExpireMinutes"];


			JWT = new clsJWT(key!, issuer!, audience!, double.Parse(expireMinutes!));

		}
	}
}
