using Daftari.Data;
using Daftari.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{

		private readonly DaftariContext _context;

		public ValuesController(DaftariContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult> GetSectores()
		{
			return Ok(await _context.Sectors.ToListAsync());
		}




	}
}
