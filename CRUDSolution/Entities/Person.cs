using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// Person Domain Model Class
    /// </summary>

    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }

        [StringLength(40)]
        public string? PersonName { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        public Guid? CountryID { get; set; }

        [StringLength(200)]
        public string? Address {  get; set; }

        public bool RecieveNewsLetters { get; set; }

        public string? TIN { get; set; }


        [ForeignKey("CountryID")]
        public Country? Country { get; set; }

        public override string ToString()
        {
            return $"PersonID={PersonId}, PersonName={PersonName}, Email={Email}, DateOfBirth={DateOfBirth}, Gender={Gender}, Countr={Country.CountryName}, Address={Address}, Receive News letters={RecieveNewsLetters} ";
        }
    }
}
