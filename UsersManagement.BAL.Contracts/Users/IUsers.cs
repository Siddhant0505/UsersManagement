using System;
using System.Collections.Generic;
using System.Text;

namespace UsersManagement.BAL.Contracts.Users
{
    public interface IUsers
    {
        bool Login(string UserName, string Password);
    }
}
