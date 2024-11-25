using Daftari.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class BusinessTypesController : BaseController
	{

		public BusinessTypesController(DaftariContext context)
			: base(context)
		{

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
