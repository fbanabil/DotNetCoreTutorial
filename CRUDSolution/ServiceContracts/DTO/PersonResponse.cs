using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool RecieveNewsLetters { get; set; }
        public Double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse other = (PersonResponse)obj;

            return this.PersonId == other.PersonId && this.PersonName == other.PersonName &&
                this.Email == other.Email && this.DateOfBirth == other.DateOfBirth && this.CountryID
                == other.CountryID && this.Address == other.Address && this.RecieveNewsLetters ==
                other.RecieveNewsLetters && this.Gender == other.Gender;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"PersonId: {PersonId}, PersonName: {PersonName}, Email: {Email}, DateOfBirth: {DateOfBirth}, " +
                   $"CountryID: {CountryID}, Address: {Address}, RecieveNewsLetters: {RecieveNewsLetters}, " +
                   $"Age: {Age} ";

        }
    }

    public static class PersonExtension
    {
        /// <summary>
        /// An extension method
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Return Converted Person Response</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                RecieveNewsLetters = person.RecieveNewsLetters,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now-person.DateOfBirth.Value).TotalDays/365.25) : null
            };
        }
    }
}
