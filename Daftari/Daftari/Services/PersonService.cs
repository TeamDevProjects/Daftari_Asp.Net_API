using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Services.HelperServices;
using Daftari.Services.IServices;

namespace Daftari.Services
{
    public class PersonService : IPersonService
    {
        protected readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }


        public async Task<bool> CheckPhoneIsExistAsync(string Phone)
        {
            // provide douplicate client phone
            var isExist = await _personRepository.CheckPhoneIsExistAsync(Phone);

            if (isExist) throw new InvalidOperationException("this phone is orleady exist");

            return false;

        }



    }
}
