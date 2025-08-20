using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Act as DTO inserting new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage ="Person Name can not be blanck")]
        public string? PersonName { get; set; }


        [Required(ErrorMessage = "Email can not be blanck")]
        [EmailAddress(ErrorMessage ="Email Adress must be email")]
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
