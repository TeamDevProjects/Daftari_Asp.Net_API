using Daftari.Data;
using Daftari.Dtos.People.Client;
using Daftari.Entities;
using Daftari.Entities.Views;
using Daftari.Interfaces;
using Daftari.Repositories;
using Daftari.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IPersonRepository _personRepository;
        private readonly DaftariContext _context;

        public ClientService( DaftariContext context,IPersonRepository personRepository, IClientRepository repository)
        {
            _clientRepository = repository;
            _personRepository = personRepository;
            _context = context;
        }

        public async Task<ClientsView> AddClientAsync(ClientCreateDto clientData, int userId)
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

            // client view contain all data nedded
            var clientView = await _context.ClientsViews.FirstOrDefaultAsync((c)=>c.ClientId == clientAdded.ClientId); 

            return clientView;
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

		public async Task<IEnumerable<ClientsView>> GetAllClients(int userId)
		{
			var clients = await _clientRepository.GetAll(userId);

			if (clients == null) throw new KeyNotFoundException($"There are no clients in database.");

			return clients;
		}
       
        public async Task<IEnumerable<ClientsView>> GetAllClientsOrderedByName(int userId)
		{
			var clients = await _clientRepository.GetAllOrderedByName(userId);

			if (clients == null) throw new KeyNotFoundException($"There are no clients in database.");

			return clients;
		}
        
        public async Task<IEnumerable<ClientsView>> SearchForClientsByName(string temp)
		{
			var clients = await _clientRepository.SearchByName(temp);

			if (clients == null) throw new KeyNotFoundException($"There are no clients in database.");

			return clients;
		}


	}
}
