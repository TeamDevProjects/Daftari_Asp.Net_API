using Daftari.Dtos.People.Client;
using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Services.HelperServices;
using Daftari.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IPersonRepository _personRepository;

        public ClientService(IPersonRepository personRepository, IClientRepository repository)
        {
            _clientRepository = repository;
            _personRepository = personRepository;
        }



        public async Task<Client> AddClientAsync(ClientCreateDto clientData, int userId)
        {
            // create Person
            var newPerson = new Person
            {
                Name = clientData.Name,
                Phone = clientData.Phone,
                City = clientData.City,
                Country = clientData.Country,
                Address = clientData.Address,
            };

            var personAdded = await _personRepository.AddAsync(newPerson);

            if (personAdded == null) throw new InvalidOperationException("can`t add this Person");

            // create Client
            var newClient = new Client
            {
                PersonId = personAdded.PersonId,
                UserId = userId,
                Notes = clientData.Notes,
            };

            var clientAdded = await _clientRepository.AddAsync(newClient);
            if (clientAdded == null) throw new InvalidOperationException("can`t add this client");

            return newClient;
        }

        public async Task<bool> UpdateClientAsync(ClientUpdateDto clientData, int clientId)
        {

            var existClient = await _clientRepository.GetByIdAsync(clientId);

            if (existClient == null) throw new KeyNotFoundException($"clientId = {clientId} is not exist");



            var person = await _personRepository.GetByIdAsync(existClient.PersonId);

            person.Name = clientData.Name;
            person.Phone = clientData.Phone;
            person.City = clientData.City;
            person.Country = clientData.Country;
            person.Address = clientData.Address;

            var personUpdated = await _personRepository.UpdateAsync(person);

            existClient.Notes = clientData.Notes;

            var clientUpdated = await _clientRepository.UpdateAsync(existClient);

            if (!clientUpdated && !personUpdated) throw new InvalidOperationException("Client added successfully");

            return true;

        }

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            var existClient = await _clientRepository.GetByIdAsync(clientId);

            if (existClient == null) throw new KeyNotFoundException($"clientId = {clientId} is not exist");


            var IsClientDeleted = await _clientRepository.DeleteAsync(existClient.ClientId);

            var IsPersonDeleted = await _personRepository.DeleteAsync(existClient.PersonId);

            if (!IsPersonDeleted || !IsClientDeleted) throw new InvalidOperationException("Unable to deleted Client");

            return true;

            // delete 
            // it`s transactions
            // it`s paymentdates
            // it`s totalamount
            // using after delete trigger from database
        }

        // get


    }
}
