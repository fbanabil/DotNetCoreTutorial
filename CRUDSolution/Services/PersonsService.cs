using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsService : IPersonsService
    {

        private readonly List<Person>? _persons;
        private readonly ICountriesService _countriesService;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsService()
        {
            _countriesService = new CountriesService();
            _persons = new List<Person>();
        }


        //Person to person resoponse method
        private async Task<PersonResponse> ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            CountryResponse? country = await _countriesService.GetCountryByCountryID(person.CountryID);
            personResponse.Country = country?.CountryName;
            return personResponse;
        }


        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //Null argument
            if(personAddRequest==null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Null Person name
            if(string.IsNullOrEmpty(personAddRequest.PersonName))
            {
                throw new ArgumentException("Person can't be blank");
            }

            //Model Validation
            ValidationHelper.ModelValidate(personAddRequest);
           

            // Create person id
            Person person = personAddRequest.ToPerson();
            
            //Assign Guid
            person.PersonId = Guid.NewGuid();

            //Add person
            _persons.Add(person);

            //Return person
            return await ConvertPersonToPersonResponse(person);
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            List<PersonResponse> personResponses = new List<PersonResponse>();
            personResponses = _persons.Select((person) => person.ToPersonResponse()).ToList();
            foreach (var personResponse in personResponses)
            {
                CountryResponse? country = await _countriesService.GetCountryByCountryID(personResponse.CountryID);
                personResponse.Country = country?.CountryName;
            }
            return personResponses;
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if(personId==null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault(p => p.PersonId == personId);

            if(person== null)
            {
                return null;
            }
            return await ConvertPersonToPersonResponse(person);
        }

        public async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons=await GetAllPersons();
            List<PersonResponse> matching = allPersons;
            if (string.IsNullOrEmpty(searchBy)|| string.IsNullOrEmpty(searchString))
            {
               return allPersons;
            }

            switch(searchBy)
            {
                case nameof(Person.PersonName):
                    matching=allPersons.Where(temp=>temp.PersonName != null && temp.PersonName!="" && 
                        temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.Email):
                    matching = allPersons.Where(temp => temp.Email != null && temp.Email!="" &&
                        temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    matching = allPersons.Where(temp => temp.DateOfBirth != null &&
                        temp.DateOfBirth.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.CountryID):
                    if (Guid.TryParse(searchString, out Guid countryId))
                    {
                        matching = allPersons.Where(temp => temp.CountryID == countryId).ToList();
                    }
                    break;
                case nameof(Person.Gender):
                    matching = allPersons.Where(temp => temp.Gender != null &&
                        temp.Gender.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.Address):
                    matching = allPersons.Where(temp => temp.Address != null && temp.Address!="" &&
                        temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    matching = allPersons;
                    break;
            }
            return matching;
        }
    }
}
