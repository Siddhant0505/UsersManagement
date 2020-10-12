using System;
using System.Collections.Generic;
using System.Text;

namespace UsersManagement.Repository.Contracts.Repository
{
    public interface IUserRepository : IRepository<UsersManagement.DAL.DatabaseEntity.Users>
    {

    }
}
