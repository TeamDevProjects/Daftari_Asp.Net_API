using Daftari.Data;
using Daftari.Dtos.People.Client;
using Daftari.Entities;
using Daftari.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Repositories
{
	public class ClientRepository:Repository<Client>, IClientRepository
	{
        public ClientRepository(DaftariContext context):base(context) { }
        
    }
}
