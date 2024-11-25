using Daftari.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class SectorsController : BaseController
	{

		public SectorsController(DaftariContext context)
			: base(context)
		{

		}

		[HttpGet]
		public async Task<IActionResult> GetSectorsWithSetctorType()
		{
			try
			{
				var sectors = await _context.SectorsViews.ToListAsync();

				if (sectors.Count == 0) 
				{
					return NoContent();
				}

				return Ok(sectors);
			}
			catch (Exception ex) 
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
