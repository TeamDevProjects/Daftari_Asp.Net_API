using Daftari.Dtos.People.Person;

namespace Daftari.Dtos.People.Supplier
{
	public class SupplierUpdateDto: PersonCreateDto
	{
        public string Notes { get; set; } = null!;
    }
}
