using Daftari.Data;
using Daftari.Dtos.People.Client;
using Daftari.Services;
using Daftari.Services.HelperServices;
using Daftari.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class ClientsController : BaseController
	{
		private readonly IClientService _clientService;
		private readonly IPersonService _PersonService;
		public ClientsController(DaftariContext context, IClientService clientService, IPersonService personService)
			: base(context)
		{
			_clientService = clientService;
			_PersonService = personService;
		}

		// Add     +
		// Update  
		// Delete  
		// Get



		[HttpPost]
		public async Task<IActionResult> PostClient([FromBody] ClientCreateDto clientData)
		{

			

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				// provide douplicate client phone
				await _PersonService.CheckPhoneIsExistAsync(clientData.Phone);

				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");
				var clientAdded = await _clientService.AddClientAsync(clientData, userId);

				// Commit the transaction if both operations succeed
				await transaction.CommitAsync();

				return Ok(clientAdded);
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();

				return BadRequest(new { error = ex.Message });
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();

				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();

				// Return an error response with the exception details
				return StatusCode(500, new { error = "An error occurred while creating the client and person.", details = ex.Message });
			}
		}

		//[HttpGet("{clientId}")]
		//public async Task<IActionResult> GetClient(int clientId)
		//{
		//	// view
		//}

		[HttpPut("{clientId}")]
		public async Task<IActionResult> UpdateClient([FromBody] ClientUpdateDto clientData, int clientId)
		{
			var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				await _clientService.UpdateClientAsync(clientData, clientId);
				
				await transaction.CommitAsync();

				return Ok("client updated successfully");
			}
			catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, new { error = "An error occurred while Updateing the client and person.", details = ex.Message });

			}
		}

		[HttpDelete("{clientId}")]
		public async Task<IActionResult> DeleteClient(int clientId)
		{
			var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				var isDeleted = await _clientService.DeleteClientAsync(clientId);
				
				await transaction.CommitAsync();
				if (isDeleted)
					return Ok("client deleted successfully");
				else return BadRequest("Unable to delete this Client");
				
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);

			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, new { error = "An error occurred while Updateing the client and person.", details = ex.Message });

			}

		}
	}
}
