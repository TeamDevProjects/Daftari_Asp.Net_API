using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Dtos.People.Person;
using Daftari.Dtos.People.Supplier;
using Daftari.Entities;
using Daftari.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SuppliersController : BasePersonsController
	{
		public SuppliersController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
		{

		}


		// Add     +
		// Update  
		// Delete  
		// Get



		[HttpPost]
		public async Task<IActionResult> PostSupplier([FromBody] SupplierCreateDto supplierData)
		{
			// provide douplicate client phone
			if (_context.People.Any(u => u.Phone == supplierData.Phone))
				return BadRequest("Supplier already exists.");

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
					Name = supplierData.Name,
					Phone = supplierData.Phone,
					City = supplierData.City,
					Country = supplierData.Country,
					Address = supplierData.Address,
				});

				if (newPerson.PersonId == 0)
				{
					return Conflict("can`t add this Person");
				}

				// create Client
				var newSupplier = new Supplier
				{
					PersonId = newPerson.PersonId,
					UserId = userId,
					Notes = supplierData.Notes,
				};

				_context.Suppliers.Add(newSupplier);
				await _context.SaveChangesAsync();

				// Commit the transaction if both operations succeed
				await transaction.CommitAsync();

				return Ok("Supplier Created successfully.");
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
