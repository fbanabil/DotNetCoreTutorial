using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Email can't be empty")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Person name can't be empty")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Phone number can't be empty")]
        [RegularExpression("^[0-9]*$",ErrorMessage ="PPhone number must contain 0-9")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password can't be empty")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [Required(ErrorMessage = "Password can't be empty")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
