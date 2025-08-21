using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{

    /// <summary>
    /// Represents DTO for updating  person details
    /// </summary>

    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Person Id can not be blank")]
        public Guid PersonId { get; set; }


        [Required(ErrorMessage = "Person Name can not be blanck")]
        public string? PersonName { get; set; }


        [Required(ErrorMessage = "Email can not be blanck")]
        [EmailAddress(ErrorMessage = "Email Adress must be email")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool RecieveNewsLetters { get; set; }


        public Person ToPerson()
        {
            return new Person()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Address = Address,
                CountryID = CountryID,
                Gender = Gender.ToString(),
                RecieveNewsLetters = RecieveNewsLetters,
            };
        }
    }
}
