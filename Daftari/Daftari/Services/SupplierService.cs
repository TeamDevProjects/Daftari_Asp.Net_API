using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Services.IServices;
using Daftari.Dtos.People.Supplier;
using Daftari.Entities.Views;
using Daftari.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services
{
    public class SupplierService : ISupplierService

    {
        protected readonly IPersonRepository _personRepository;
        protected readonly ISupplierRepository _supplierRepository;
        protected readonly DaftariContext _context;

        public SupplierService(DaftariContext context,IPersonRepository personRepository, ISupplierRepository supplierRepository)
        {
            _personRepository = personRepository;
            _supplierRepository = supplierRepository;
            _context = context;
        }



        public async Task<SuppliersView> AddSupplierAsync(SupplierCreateDto SupplierData, int userId)
        {
            // create Person
            var newPerson = new Person
            {
                Name = SupplierData.Name,
                Phone = SupplierData.Phone,
                City = SupplierData.City,
                Country = SupplierData.Country,
                Address = SupplierData.Address,
            };
            var personAdded = await _personRepository.AddAsync(newPerson);
            if (personAdded == null) throw new InvalidOperationException("can`t add this Person");

            // create Supplier
            var newSupplier = new Supplier
            {
                PersonId = newPerson.PersonId,
                UserId = userId,
                Notes = SupplierData.Notes,
            };
            var supplierAdded = await _supplierRepository.AddAsync(newSupplier);
            if (supplierAdded == null) throw new InvalidOperationException("can`t add this Supplier");

            var supplierView = await _context.SuppliersViews.FirstOrDefaultAsync((c) => c.SupplierId == supplierAdded.SupplierId);

            return supplierView;
        }

        public async Task<bool> UpdateSupplierAsync(SupplierUpdateDto SupplierData, int supplierId)
        {

            var existSupplier = await _supplierRepository.GetByIdAsync(supplierId);

            if (existSupplier == null) throw new InvalidOperationException($"SupplierId = {supplierId} is not exist");


            var person = await _personRepository.GetByIdAsync(existSupplier.PersonId);

            person.Name = SupplierData.Name;
            person.Phone = SupplierData.Phone;
            person.City = SupplierData.City;
            person.Country = SupplierData.Country;
            person.Address = SupplierData.Address;

            var personUpdated = await _personRepository.UpdateAsync(person);

            existSupplier.Notes = SupplierData.Notes;

            var supplierUpdated = true; //await UpdateSupplierAsync(SupplierData, supplierId);

            if (!supplierUpdated && !personUpdated) throw new InvalidOperationException("Unable to update supplier");

            return true;

        }

        public async Task<bool> DeleteSupplierAsync(int SupplierId)
        {
            var existSupplier = await _supplierRepository.GetByIdAsync(SupplierId);

            if (existSupplier == null)
            {
                throw new InvalidOperationException($"SupplierId = {SupplierId} is not exist");
            }

            var IsSupplierDeleted = await _supplierRepository.DeleteAsync(existSupplier.SupplierId);

            var IsPersonDeleted = await _personRepository.DeleteAsync(existSupplier.PersonId);

            if (!IsPersonDeleted || !IsSupplierDeleted) throw new InvalidOperationException("Unable to delete supplier");

            return true;

            // delete
            // it`s transactions
            // it`s paymentdates
            // it`s totalamount
        }

		public async Task<IEnumerable<SuppliersView>> GetAllSuppliers(int userId)
		{
			var suppliers = await _supplierRepository.GetAll(userId);

			if (suppliers == null) throw new KeyNotFoundException($"There are no clients in database.");

			return suppliers;
		}

		public async Task<IEnumerable<SuppliersView>> GetAllClientsOrderedByName(int userId)
		{
			var suppliers = await _supplierRepository.GetAllOrderedByName(userId);

			if (suppliers == null) throw new KeyNotFoundException($"There are no clients in database.");

			return suppliers;
		}

		public async Task<IEnumerable<SuppliersView>> SearchForClientsByName(string temp)
		{
			var suppliers = await _supplierRepository.SearchByName(temp);

			if (suppliers == null) throw new KeyNotFoundException($"There are no clients in database.");

			return suppliers;
		}


	}
}
