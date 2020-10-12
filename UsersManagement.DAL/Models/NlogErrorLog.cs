using System;
using System.Collections.Generic;

namespace UsersManagement.DAL.Models
{
    public partial class NlogErrorLog
    {
        public int ErrorId { get; set; }
        public DateTime ErrorDatetime { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorLevel { get; set; }
        public string ErrorMessage { get; set; }
    }
}
