using Daftari.Data;
using Daftari.Entities;
using Daftari.Entities.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SectorController : ControllerBase
	{
		private readonly DaftariContext _context;

		public SectorController(DaftariContext context)
		{
			_context = context;
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
