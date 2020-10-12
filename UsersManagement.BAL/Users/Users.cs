using System;
using System.Collections.Generic;
using System.Text;
using UsersManagement.BAL.Contracts.Users;
using UsersManagement.Repository.Common;
using UsersManagement.Repository.Contracts.UnitOfWork;

namespace UsersManagement.BAL.Users
{
    public class Users : IUsers
    {
        private readonly IUnitOfWork uow;
        public Users(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public bool Login(string UserName, string Password)
        {
            
            var sd= uow.User.GetAll();
            return true;
                
        }
    }
}
