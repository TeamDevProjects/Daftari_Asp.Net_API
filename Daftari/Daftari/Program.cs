
using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Middleware;
using Daftari.Repositories;
using Daftari.Services;
using Daftari.Services.HelperServices;
using Daftari.Services.InterfacesServices;
using Daftari.Services.IServices;
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


			// Register Controllers using Dependence Injection
			builder.Services.AddScoped<JwtHelper>();

			

			// Add PersonService and Repositories
			builder.Services.AddScoped<IPersonService, PersonService>(); // Register PersonService
			builder.Services.AddScoped<IClientService,ClientService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<ISupplierService,SupplierService>();
			

			builder.Services.AddScoped<IPersonRepository, PersonRepository>();
			builder.Services.AddScoped<IClientRepository, ClientRepository>();
			builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();

			builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
			builder.Services.AddScoped<IUserTransactionRepository, UserTransactionRepository>();
			builder.Services.AddScoped<IClientTransactionRepository, ClientTransactionRepository>();
			builder.Services.AddScoped<ISupplierTransactionRepository, SupplierTransactionRepository>();
			builder.Services.AddScoped<IPaymentDateRepository, PaymentDateRepository>();
			builder.Services.AddScoped<IClientPaymentDateRepository, ClientPaymentDateRepository>();
			builder.Services.AddScoped<ISupplierPaymentDateRepository, SupplierPaymentDateRepository>();
			builder.Services.AddScoped<IClientTotalAmountRepository, ClientTotalAmountRepository>();
			builder.Services.AddScoped<IUserTotalAmountRepository, UserTotalAmountRepository>();
			builder.Services.AddScoped<ISupplierTotalAmountRepository, SupplierTotalAmountRepository>();

			builder.Services.AddScoped<IPaymentDateService,PaymentDateService>();
			builder.Services.AddScoped<IClientPaymentDateService,ClientPaymentDateService>();
			builder.Services.AddScoped<ISupplierPaymentDateService,SupplierPaymentDateService>();

			builder.Services.AddScoped<IClientTotalAmountService, ClientTotalAmountService>();
			builder.Services.AddScoped<IUserTotalAmountService, UserTotalAmountService>();
			builder.Services.AddScoped<ISupplierTotalAmountService, SupplierTotalAmountService>();

			builder.Services.AddScoped<IClientTransactionService, ClientTransactionService>();
			builder.Services.AddScoped<IUserTransactionService, UserTransactionService>();
			builder.Services.AddScoped<ISupplierTransactionService, SupplierTransactionService>();




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

			app.UseMiddleware<ExceptionHandlingMiddleware>();
			app.UseAuthentication(); // to use jwt
			
			app.UseAuthorization();


			app.MapControllers();


			app.Run();
			
			
		}
	}
}
