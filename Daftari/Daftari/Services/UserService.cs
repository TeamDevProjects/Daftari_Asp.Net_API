using Daftari.Dtos.People.User;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Daftari.Services.HelperServices;
using Daftari.Services.InterfacesServices;

namespace Daftari.Services
{
    public class UserService : IUserService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;

        public UserService(IPersonRepository personRepository, IUserRepository userRepository)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> AddUserAsync(UserCreateDto userData)
        {
            // create Person
            var newPerson = new Person
            {
                Name = userData.Name,
                Phone = userData.Phone,
                City = userData.City,
                Country = userData.Country,
                Address = userData.Address,
            };
            var clientAdded = await _personRepository.AddAsync(newPerson);
            if (clientAdded == null) throw new InvalidOperationException("Unable to add Person");

            // create User
            var newUser = new User
            {
                UserName = userData.UserName,
                PasswordHash = PasswordHelper.HashingPassword(userData.PasswordHash),
                PersonId = newPerson.PersonId,
                SectorId = userData.SectorId,
                BusinessTypeId = userData.BusinessTypeId,
                StoreName = userData.StoreName,
                UserType = userData.UserType,
            };
            var userAdded = await _userRepository.AddAsync(newUser);
            if (userAdded == null) throw new InvalidOperationException("Unable to add User");

            return true;
        }

        // login 

        public async Task<bool> UpdateUserAsync(UserUpdateDto userData, int UserId)
        {

            var existUser = await _userRepository.GetByIdAsync(UserId);

            if (existUser == null) throw new KeyNotFoundException($"UserId = {UserId} is not exist");


            var person = await _personRepository.GetByIdAsync(existUser.PersonId);

            person.Name = userData.Name;
            person.Phone = userData.Phone;
            person.City = userData.City;
            person.Country = userData.Country;
            person.Address = userData.Address;

            var personUpdated = await _personRepository.UpdateAsync(person);


            existUser.StoreName = userData.StoreName;
            existUser.UserName = userData.UserName;
            existUser.PasswordHash = PasswordHelper.HashingPassword(userData.PasswordHash);

            var userUpdated = await _userRepository.UpdateAsync(existUser);

            if (!userUpdated && !personUpdated) throw new InvalidOperationException("User updated successfully");

            return true;
        }

        public async Task<bool> DeleteUserAsync(int UserId)
        {
            var existUser = await _userRepository.GetByIdAsync(UserId);

            if (existUser == null) throw new KeyNotFoundException($"UserId = {UserId} is not exist");


            var IsUserDeleted = await _userRepository.DeleteAsync(existUser.UserId);

            var IsPersonDeleted = await _personRepository.DeleteAsync(existUser.PersonId);

            if (!IsPersonDeleted || !IsUserDeleted) throw new InvalidOperationException("User deleted successfully");

            return true;

            // delete
            // it`s transactions
            // it`s paymentdates
            // it`s totalamount
            // after deleted trigger from database
        }

        public async Task<IEnumerable<UsersView>> GetAll()
        {
            var Users = await _userRepository.GetAll();

            if (Users == null) throw new KeyNotFoundException($"There are no users in database.");

            return Users;
        }
        
        public async Task<IEnumerable<UsersView>> SearchForUsersByName(string temp)
        {
            var Users = await _userRepository.SearchByName(temp);

            if (Users == null) throw new KeyNotFoundException($"There are no user has name = {temp}.");

            return Users;
        }
    }
}
