using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.People.Person;
using Daftari.Dtos.People.User;
using Daftari.Entities;
using Daftari.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UsersController : BasePersonsController
	{

		public UsersController(DaftariContext context, JwtHelper jwtHelper)
			:base(context, jwtHelper)
		{
			
		}


		[Authorize(Roles = "admin")]
		[HttpGet]
		public async Task<ActionResult> GetUsers()
		{
			return Ok(await _context.Users.ToListAsync());
		}


		[HttpPost("signup")]
		public async Task<IActionResult> Register([FromBody] UserCreateDto userData)
		{
			if (_context.Users.Any(u => u.UserName == userData.UserName))
				return BadRequest("Username already exists.");

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{

				// create Person
				var newPerson = await CreatePerson(new PersonCreateDto
				{
					Name = userData.Name,
					Phone = userData.Phone,
					City = userData.City,
					Country = userData.Country,
					Address = userData.Address,
				});

				if (newPerson.PersonId == 0)
				{
					return Conflict("can`t add this Person");
				}

				// create User
				var newUser = new User
				{
					UserName = userData.UserName,
					PasswordHash = PasswordHelper.HashingPassword(userData.PasswordHash),
					PersonId = newPerson.PersonId,
					SectorId = userData.SectorId,
					BusinessTypeId = userData.BusinessTypeId,
					StoreName = userData.StoreName,
					UserType = userData.UserType,
				};

				_context.Users.Add(newUser);
				await _context.SaveChangesAsync();

				// Commit the transaction if both operations succeed
				await transaction.CommitAsync();

				return Ok("User registered successfully.");
			}
			catch (Exception ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();

				// Return an error response with the exception details
				return StatusCode(500, new { error = "An error occurred while creating the user and person.", details = ex.Message });
			}
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDto userData)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == userData.UserName);

			if (user == null || !PasswordHelper.VerifyPassword(userData.PasswordHash, user.PasswordHash))
				return Unauthorized("Invalid username or password.");

			var accessToken = _jwtHelper.GenerateToken(user.UserId.ToString(), user.UserName, user.UserType);
			var refreshToken = _jwtHelper.GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 
			await _context.SaveChangesAsync();

			return Ok(new
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			});

			//AhmedEid => admin
			//EidAhmed => user
		}

		// Update
		// Delete
		// Get

		[Authorize]
		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

			if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
				return Unauthorized("Invalid refresh token or token has expired.");

			var newAccessToken = _jwtHelper.GenerateToken(user.UserId.ToString(), user.UserName, user.UserType);

			var newRefreshToken = _jwtHelper.GenerateRefreshToken();
			user.RefreshToken = newRefreshToken;
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				AccessToken = newAccessToken,
				RefreshToken = newRefreshToken
			});
		}

	}
}
