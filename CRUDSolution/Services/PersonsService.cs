using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
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

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> personResponses, string sortBy, SortOrderOptions sortOrder)
        {
            if(string.IsNullOrEmpty(sortBy) || personResponses == null || personResponses.Count == 0)
            {
                return personResponses;
            }

            // Sort the personResponses based on the sortBy parameter

            List<PersonResponse> sortedResponse = (sortBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.PersonName,StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) 
                    => personResponses.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.Email), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Age).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.RecieveNewsLetters).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.RecieveNewsLetters).ToList(),
                
                _=> personResponses
            };

            return sortedResponse;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest==null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            if(personUpdateRequest.PersonName==null)
            {
                throw new ArgumentException(nameof(personUpdateRequest.PersonName));
            }

            ValidationHelper.ModelValidate(personUpdateRequest);

            Person? personResponse = _persons.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);

            if(personResponse==null)
            {
                throw new ArgumentException(nameof(personResponse));
            }

            
            personResponse.PersonName = personUpdateRequest.PersonName;
            personResponse.Email = personUpdateRequest.Email;
            personResponse.DateOfBirth = personUpdateRequest.DateOfBirth;
            personResponse.Gender = personUpdateRequest.Gender.ToString();
            personResponse.CountryID = personUpdateRequest.CountryID;
            personResponse.Address = personUpdateRequest.Address;
            personResponse.RecieveNewsLetters = personUpdateRequest.RecieveNewsLetters;

            return personResponse.ToPersonResponse();


        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if(personId==null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = _persons.FirstOrDefault(p => p.PersonId == personId);

            if (person == null)
            {
                return false;
            }

            bool isRemoved = _persons.Remove(person);
            if (isRemoved)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
