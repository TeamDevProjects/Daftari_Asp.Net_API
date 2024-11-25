using Daftari.Dtos.People.Person;

namespace Daftari.Dtos.People.Supplier
{
    public class SupplierCreateDto : PersonCreateDto
    {
        public string Notes { get; set; } = null!;


    }
}
