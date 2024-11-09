using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SectorController : BaseController
	{

		public SectorController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
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
