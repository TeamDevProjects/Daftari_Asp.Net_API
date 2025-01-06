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
		public async Task<IActionResult> UpdateSupplier([FromBody] SupplierUpdateDto SupplierData,int SupplierId)
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

		[HttpGet("search/{temp}")]
		public async Task<ActionResult> SearchForsuppliersByName(string temp)
		{
			try
			{
				var suppliers = await _supplierService.SearchForSuppliers(temp);

				return Ok(suppliers);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) when (ex.Message.Contains("No Content"))
			{
				throw new Exception("No Content");

			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}

		}

		[HttpGet("orderBy/Name")]
		public async Task<ActionResult> GetOrderedSuppliersByName()
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");

				var suppliers = await _supplierService.GetAllOrderedByName(userId);
				
				return Ok(suppliers);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) when (ex.Message.Contains("No Content"))
			{
				throw new Exception("No Content");

			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}
		}

		//GetAllOrderedByCloserPaymentDates
		[HttpGet("orderBy/CloserPaymentDates")]
		public async Task<ActionResult> GetOrderedByCloserPaymentDates()
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");

				var suppliers = await _supplierService.GetAllOrderedByCloserPaymentDates(userId);
				
				return Ok(suppliers);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) when (ex.Message.Contains("No Content"))
			{
				throw new Exception("No Content");

			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}
		}

		//GetAllOrderedByOlderPaymentDates
		[HttpGet("orderBy/OlderPaymentDates")]
		public async Task<ActionResult> GetOrderedByOlderPaymentDates()
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");

				var suppliers = await _supplierService.GetAllOrderedByOlderPaymentDates(userId);
				
				return Ok(suppliers);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) when (ex.Message.Contains("No Content"))
			{
				throw new Exception("No Content");

			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}
		}

		//GetAllOrderedByLargestTotalAmount
		[HttpGet("orderBy/LargestTotalAmount")]
		public async Task<ActionResult> GetOrderedByLargestTotalAmount()
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");

				var suppliers = await _supplierService.GetAllOrderedByLargestTotalAmount(userId);
				
				return Ok(suppliers);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) when (ex.Message.Contains("No Content"))
			{
				throw new Exception("No Content");

			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}
		}

		//GetAllOrderedBySmallestTotalAmount
		[HttpGet("orderBy/SmallestTotalAmount")]
		public async Task<ActionResult> GetOrderedBySmallestTotalAmount()
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");

				var suppliers = await _supplierService.GetAllOrderedBySmallestTotalAmount(userId);
				
				return Ok(suppliers);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) when (ex.Message.Contains("No Content"))
			{
				throw new Exception("No Content");

			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}
		}


		[HttpGet]
		public async Task<ActionResult> GetAllClients()
		{
			try
			{
				// Get UserId from header request from token
				var userId = GetUserIdFromToken();

				if (userId == -1) return Unauthorized("UserId is not founded in token");

				var suppliers = await _supplierService.GetAllSuppliers(userId);

				return Ok(suppliers);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);

			}
			catch (Exception ex) when (ex.Message.Contains("No Content"))
			{
				 throw new Exception("No Content");

			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while refresh token.", details = ex.Message });
			}
		}



	}
}
