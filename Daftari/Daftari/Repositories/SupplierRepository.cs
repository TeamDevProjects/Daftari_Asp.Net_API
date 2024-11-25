using Daftari.Data;
using Daftari.Dtos.People.Supplier;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class SupplierRepository : Repository<Supplier>, ISupplierRepository
	{
		public SupplierRepository(DaftariContext context) : base(context) { }

	}
}
