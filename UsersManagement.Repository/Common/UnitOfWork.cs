using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.Repository.Contracts.Repository;
using UsersManagement.Repository.Contracts.UnitOfWork;
using UsersManagement.Repository.Repository;

namespace UsersManagement.Repository.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UsersManagement.DAL.DatabaseEntity.UserManagementContext _dbContext;
       
        public UnitOfWork(UsersManagement.DAL.DatabaseEntity.UserManagementContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private IUserRepository _User;

        public IUserRepository User
        {
            get
            {
                if (this._User == null)
                {
                    this._User = new UserRepository(_dbContext);
                }
                return this._User;
            }
        }

        public int SaveChanges()
        {
            return this._dbContext.SaveChanges();
        }
        public async virtual Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }


        public void Dispose()
        {
            this._dbContext?.Dispose();
        }

        //public IDatabaseTransaction BeginTransaction()
        //{
        //    return new EntityDatabaseTransaction(_dbContext);
        //}
    }
}
