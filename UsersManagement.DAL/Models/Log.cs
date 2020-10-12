using System;
using System.Collections.Generic;

namespace UsersManagement.DAL.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public string Application { get; set; }
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
