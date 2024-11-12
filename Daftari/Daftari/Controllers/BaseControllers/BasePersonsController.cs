using Daftari.Data;
using Daftari.Dtos.People.Person;
using Daftari.Entities;
using Daftari.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers.BaseControllers
{
    public class BasePersonsController : BaseController
	{
		public BasePersonsController(DaftariContext context, JwtHelper jwtHelper)
			: base(context, jwtHelper)
		{

		}

		protected async Task<Person> CreatePerson(PersonCreateDto personData)
		{

			// create Person
			var newPerson = new Person
			{
				Name = personData.Name,
				Phone = personData.Phone,
				City = personData.City,
				Country = personData.Country,
				Address = personData.Address,
			};


			await _context.People.AddAsync(newPerson);
			await _context.SaveChangesAsync();

			return newPerson;
		}

	}
}
