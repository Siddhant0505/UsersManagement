﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UsersManagement.Domain.Home
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
