using Daftari.Dtos.People.Person;

namespace Daftari.Dtos.People.Client
{
    public class ClientCreateDto : PersonCreateDto
    {
        public string Notes { get; set; } = null!;

        // public int UserId { get; set; } must come from tokent in request header
    }
}
