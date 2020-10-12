using System;
using System.Collections.Generic;
using System.Text;

namespace UsersManagement.Domain.Home
{
    public class User
    {
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAILID { get; set; }
        public string PHONE { get; set; }
        public string ACCESS_LEVEL { get; set; }
        public string READ_ONLY { get; set; }
        public string WRITE_ACCESS { get; set; }

        public string Token { get; set; }
    }
}
