using System;
using System.Collections.Generic;
using System.Text;
using UsersManagement.DAL.DatabaseEntity;
using UsersManagement.Repository.Common;
using UsersManagement.Repository.Contracts.Repository;

namespace UsersManagement.Repository.Repository
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        private readonly UserManagementContext _dbContext;
        public UserRepository(UserManagementContext context) : base(context)
        {
            this._dbContext = context;
        }

        

    }
}
