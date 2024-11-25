using Daftari.Dtos.People.Person;

namespace Daftari.Dtos.People.Client
{
	public class ClientUpdateDto: PersonUpdateDto
	{
		public string Notes { get; set; } = null!;

	}
}
