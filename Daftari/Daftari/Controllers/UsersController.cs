using Daftari.Data;
using Daftari.Dtos.People.User;
using Daftari.Services.HelperServices;
using Daftari.Services.InterfacesServices;
using Daftari.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UsersController : BaseController
	{
		private readonly IUserService _userService;
		private readonly IPersonService _PersonService;
		private readonly JwtHelper _jwtHelper;

		public UsersController(DaftariContext context, JwtHelper jwtHelper, IUserService userService, IPersonService personService)
			: base(context)
		{
			_jwtHelper = jwtHelper;
			_userService = userService;
			_PersonService = personService;
		}


		
		[HttpPost("signup")]
		public async Task<IActionResult> Register([FromBody] UserCreateDto userData)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				if (_context.Users.Any(u => u.UserName == userData.UserName))
					return BadRequest("Username already exists.");

				// provide douplicate Supplier phone
				await _PersonService.CheckPhoneIsExistAsync(userData.Phone);

				// create User
				var isAdded = await _userService.AddUserAsync(userData);

				// Commit the transaction if both operations succeed
				await transaction.CommitAsync();

				return Ok("User created successfully");
			}
			catch (InvalidOperationException ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();
				return StatusCode(500, new { error = "An error occurred while creating the user and person.", details = ex.Message });
			}
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDto userData)
		{
			try
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
			}
			catch (Exception ex) 
			{
				return StatusCode(500, new { error = "An error occurred while login the user and person.", details = ex.Message });
			}

			//AhmedEid => admin
			//EidAhmed => user
		}


		[Authorize]
		[HttpPut]
		public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto UserData)
		{
			var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized($"userId = {userId} not found");

				await _userService.UpdateUserAsync(UserData, userId);
				await transaction.CommitAsync();

				return Ok("User Updated Succeffuly");
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, new { error = "An error occurred while Updateing the User and person.", details = ex.Message });
			}
		}

		[Authorize]
		[HttpDelete]
		public async Task<IActionResult> DeleteUser()
		{
			var transaction = _context.Database.BeginTransaction();

			try
			{
				var userId = GetUserIdFromToken();
				if (userId == -1) return Unauthorized();

				var isDeleted = await _userService.DeleteUserAsync(userId);
				
				await transaction.CommitAsync();

				if (isDeleted) return Ok("user deleted successfully");

				else return BadRequest("Unable to delete User");
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message );
			}catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, new { error = "An error occurred while Updateing the User and person.", details = ex.Message });
			}

		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
		{
			try
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
			catch (Exception ex) 
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}
		}



		[Authorize(Roles = "admin")]
		[HttpGet]
		public async Task<ActionResult> GetUsers()
		{
			try
			{
				var users = await _userService.GetAll();

				return Ok(users);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}
		}

		[Authorize(Roles = "admin")]
		[HttpGet("search/{temp}")]
		public async Task<ActionResult> SearchForUsersByName(string temp)
		{
			try 
			{
				var users =  await _userService.SearchForUsersByName(temp);

				return Ok(users);
			}
			catch(KeyNotFoundException ex) 
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) 
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}

		}

		[Authorize]
		[HttpGet("UserView")]
		public async Task<ActionResult> GetUserView()
		{
			try 
			{
				var userId = GetUserIdFromToken();
				if (userId == -1) return Unauthorized();

				var users =  await _userService.GetUserView(userId);

				return Ok(users);
			}
			catch(KeyNotFoundException ex) 
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) 
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}

		}



	}
}
