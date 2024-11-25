using Daftari.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ClientTotalAmountsController : BaseController
	{
		public ClientTotalAmountsController(DaftariContext context)
			: base(context)
		{
			// Get 
		}
	}
}
