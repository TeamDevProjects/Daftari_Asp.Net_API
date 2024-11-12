using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.People.Client;
using Daftari.Dtos.People.Person;
using Daftari.Entities;
using Daftari.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ClientsController : BasePersonsController
	{

		public ClientsController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
		{

		}

		// Add     +
		// Update  
		// Delete  
		// Get
		
		

		[HttpPost]
		public async Task<IActionResult> PostClient([FromBody] ClientCreateDto clientData)
		{
			// provide douplicate client phone
			if (_context.People.Any(u => u.Phone == clientData.Phone))
				return BadRequest("Client already exists.");

			// Get UserId from header request from token
			var userId = GetUserIdFromToken();

			if (userId == -1) 
			{
				return Unauthorized("UserId is not founded in token");
			}

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				// create Person
				var newPerson = await CreatePerson(new PersonCreateDto
				{
					Name = clientData.Name,
					Phone = clientData.Phone,
					City = clientData.City,
					Country = clientData.Country,
					Address = clientData.Address,
				});

				if (newPerson.PersonId == 0)
				{
					return Conflict("can`t add this Person");
				}

				// create Client
				var newClient = new Client
				{
					PersonId = newPerson.PersonId,
					UserId = userId,
					Notes  = clientData.Notes,
				};

				_context.Clients.Add(newClient);
				await _context.SaveChangesAsync();

				// Commit the transaction if both operations succeed
				await transaction.CommitAsync();

				return Ok("Client Created successfully.");
			}
			catch (Exception ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();

				// Return an error response with the exception details
				return StatusCode(500, new { error = "An error occurred while creating the user and person.", details = ex.Message });
			}
		}


	
	
	
	}
}
