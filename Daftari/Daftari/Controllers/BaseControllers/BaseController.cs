using Daftari.Data;
using Daftari.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers.BaseControllers
{
	public class BaseController : ControllerBase
	{
		protected readonly DaftariContext _context;
		protected readonly JwtHelper _jwtHelper;

		public BaseController(DaftariContext context, JwtHelper jwtHelper)
		{
			_context = context;
			_jwtHelper = jwtHelper;
		}

		protected int GetUserIdFromToken()
		{
			var userId = User.FindFirst("UserId")!.Value;

			if (userId == null) 
			{
				return -1;
			}

			return int.Parse(userId);
		}


	}
}
