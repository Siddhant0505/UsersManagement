using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UsersManagement.Repository.Contracts.Repository;

namespace UsersManagement.Repository.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        //IDatabaseTransaction BeginTransaction();
        int SaveChanges();
        IUserRepository User { get; }
    }
}
