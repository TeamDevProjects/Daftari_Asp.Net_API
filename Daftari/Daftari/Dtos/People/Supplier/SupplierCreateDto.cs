using Daftari.Dtos.People.Person;

namespace Daftari.Dtos.People.Supplier
{
    public class SupplierCreateDto : PersonCreateDto
    {
        public string Notes { get; set; } = null!;

        // public int UserId { get; set; } must come from tokent in request header

    }
}
