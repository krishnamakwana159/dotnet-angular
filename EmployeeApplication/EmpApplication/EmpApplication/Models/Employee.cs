using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpApplication.Models
{
    public class Employee
    {
        [Required]
        [StringLength(128, MinimumLength=2)]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{10}$", ErrorMessage = "Rquired Alphanumeric value of length 10")]
        public string EmployeeId { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid mobile number.")]
        public string Mobile { get; set; }

        [Required]
        public string Gender { get; set; }
        
        [Required]
        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int Age { get; set; }
    }
}
