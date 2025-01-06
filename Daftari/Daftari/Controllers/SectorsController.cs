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

				return Ok(sectors);
			}
			catch (Exception ex) when (ex.Message.Contains("No Content"))
			{
				throw new Exception("No Content");

			}
			catch (Exception ex) 
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
