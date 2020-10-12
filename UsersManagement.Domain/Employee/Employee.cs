using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UsersManagement.Domain.Employee
{
    public class Employee
    {
        [Required]
        [Display(Name = "Employee Name")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        
        [Required]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(3000, 10000000, ErrorMessage = "Salary must be between 3000 and 10000000")]
        public string Salary { get; set; }

        //[Required(ErrorMessage = "Please choose profile image")]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
    }
}
