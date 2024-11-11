﻿using Daftari.Controllers.BaseControllers;
using Daftari.Data;
using Daftari.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientTotalAmountController : BaseController
	{
		public ClientTotalAmountController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
		{

		}
	}
}
