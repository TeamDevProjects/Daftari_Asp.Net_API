﻿using Daftari.Dtos.People.User;
using Daftari.Entities;
using Daftari.Entities.Views;

namespace Daftari.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		Task<IEnumerable<UsersView>> GetAll();
		Task<IEnumerable<UsersView>> SearchByName(string temp);
	}
}
