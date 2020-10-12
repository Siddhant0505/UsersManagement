using System;
using System.Collections.Generic;

namespace UsersManagement.DAL.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
