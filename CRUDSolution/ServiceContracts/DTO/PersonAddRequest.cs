using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Act as DTO inserting new person
    /// </summary>
    public class PersonAddRequest:IValidatableObject
    {
        [Required(ErrorMessage ="Person Name can not be blanck")]
        public string? PersonName { get; set; }


        [Required(ErrorMessage = "Email can not be blank")]
        [EmailAddress(ErrorMessage ="Email Adress must be email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Date Of Birth can not be blank")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender can not be blank")]
        public GenderOptions? Gender { get; set; }

        [Required(ErrorMessage = "Country can not be blank")]
        public Guid? CountryID { get; set; }

        [Required(ErrorMessage = "Address can not be blank")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Receive News Letter can not be blank")]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DateOfBirth == null)
            {
                yield return new ValidationResult("Date of Birth can't be null");
            }
            if(DateTime.Parse(DateOfBirth.ToString()) > DateTime.Parse("01-01-2010"))
            {
                yield return new ValidationResult("Year can be at most 2010",new[] { nameof(DateOfBirth) });
            }
        }
    }
}
