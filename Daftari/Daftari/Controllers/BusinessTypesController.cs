using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BusinessTypesController : BaseController
	{

		public BusinessTypesController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
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
