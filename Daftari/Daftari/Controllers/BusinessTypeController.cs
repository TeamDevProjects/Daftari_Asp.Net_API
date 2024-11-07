using Daftari.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BusinessTypeController : ControllerBase
	{
		private readonly DaftariContext _context;

		public BusinessTypeController(DaftariContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetBusinessTypes() 
		{
			try
			{
				var businessTypes = await _context.BusinessTypes.ToListAsync();

				if (businessTypes.Count == 0)
				{
					return NoContent();
				}

				return Ok(businessTypes);
			}
			
			catch (Exception ex) 
			{
				return BadRequest(ex.Message);
			}
			
		}
	}
}
