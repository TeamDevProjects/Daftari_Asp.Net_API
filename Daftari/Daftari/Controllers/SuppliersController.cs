using Daftari.Data;
using Daftari.Dtos.People.Supplier;
using Daftari.Services;
using Daftari.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class SuppliersController :BaseController
	{
		private readonly ISupplierService _supplierService;
		private readonly IPersonService _PersonService;

		public SuppliersController(DaftariContext context, ISupplierService supplierService, IPersonService personService)
			: base(context)
		{
			_supplierService = supplierService;
			_PersonService = personService;
		}


		// Add     +
		// Update  
		// Delete  
		// Get



		[HttpPost]
		public async Task<IActionResult> PostSupplier([FromBody] SupplierCreateDto supplierData)
		{

			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{

				// provide douplicate Supplier phone
				await _PersonService.CheckPhoneIsExistAsync(supplierData.Phone);

				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");

				var supplier = await _supplierService.AddSupplierAsync(supplierData, userId);

				// Commit the transaction if both operations succeed
				await transaction.CommitAsync();

				return Ok(supplier);
			}
			catch (KeyNotFoundException ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();
				return NotFound(ex.Message);
			}catch (InvalidOperationException ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}catch (Exception ex)
			{
				// Rollback the transaction if any error occurs
				await transaction.RollbackAsync();
				return StatusCode(500, new { error = "An error occurred while creating the user and person.", details = ex.Message });
			}
		}

		[HttpPut("{SupplierId}")]
		public async Task<IActionResult> UpdateSupplier([FromBody] SupplierUpdateDto SupplierData, int SupplierId)
		{
			var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				await _supplierService.UpdateSupplierAsync(SupplierData, SupplierId);

				await transaction.CommitAsync();

				return Ok("supplier updated successfully");
			}
			catch (InvalidOperationException ex)
			{
				await transaction.RollbackAsync();
				return BadRequest(ex.Message);
			}catch (KeyNotFoundException ex)
			{
				await transaction.RollbackAsync();
				return NotFound(ex.Message) ;
			}catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, new { error = "An error occurred while Updateing the Supplier and person.", details = ex.Message });

			}
		}

		[HttpDelete("{SupplierId}")]
		public async Task<IActionResult> DeleteSupplier(int SupplierId)
		{
			var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				await _supplierService.DeleteSupplierAsync(SupplierId);

				await transaction.CommitAsync();
				return Ok("supplier deleted successfully");

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
				return StatusCode(500, new { error = "An error occurred while Updateing the Supplier and person.", details = ex.Message });
			}

		}


	}
}
