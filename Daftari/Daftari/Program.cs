
using Daftari.Data;
using Daftari.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Daftari
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();


			// Register DbContext with the configuration
			builder.Services.AddDbContext<DaftariContext>(options =>
				options.UseSqlServer(Settings.ConnectionString));


			// إعدادات JWT
			var jwtSettings = builder.Configuration.GetSection("Jwt");
			var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = jwtSettings["Issuer"],
					ValidateAudience = true,
					ValidAudience = jwtSettings["Audience"],
					ValidateLifetime = true,
				};
			});

			builder.Services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					In = Microsoft.OpenApi.Models.ParameterLocation.Header,
					Description = "Please insert JWT with Bearer into field",
					Name = "Authorization",
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
				});
				c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
				{
					new Microsoft.OpenApi.Models.OpenApiSecurityScheme
					{
					  Reference = new Microsoft.OpenApi.Models.OpenApiReference
					  {
						Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
						Id = "Bearer"
					  }
					},
					new string[] { }
				}
				});
			});

			builder.Services.AddScoped<JwtHelper>();

			builder.Services.AddAuthorization();
			builder.Services.AddControllers();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage(); // Enable developer exception page for detailed errors
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication(); // to use jwt
			
			app.UseAuthorization();


			app.MapControllers();


			app.Run();
			
			
		}
	}
}
