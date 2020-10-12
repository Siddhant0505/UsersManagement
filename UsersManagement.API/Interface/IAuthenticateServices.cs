using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersManagement.Domain.Home;

namespace UsersManagement.API.Interface
{
    public interface IAuthenticateServices
    {
        Task<User> Authenticate(string UserName, string Password);
    }
}
