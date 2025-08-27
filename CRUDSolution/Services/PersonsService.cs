using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;


namespace Services
{
    public class PersonsService : IPersonsService
    {

        private readonly PersonsDbContext _db;
        private readonly ICountriesService _countriesService;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsService(PersonsDbContext db, ICountriesService countriesService)
        {
            _countriesService = countriesService;
            _db = db;
        }
    


        //Person to person resoponse method
        private async Task<PersonResponse> ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = person.Country?.CountryName;
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
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();

            //_db.sp_InsertPerson(person);

            //Return person
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            //List<Person> persons = new List<Person>();
            //persons = _db.sp_GetAllPersons();

            //List<PersonResponse> personResponses= new List<PersonResponse>();

            //foreach (Person person in persons)
            //{
            //    personResponses.Add(await ConvertPersonToPersonResponse(person));
            //}

            //return personResponses;

            //var persons = await _db.Persons.Include(temp => temp.Country).ToListAsync();

            return await _db.Persons.Include(temp => temp.Country)
               .Select(person => person.ToPersonResponse()).ToListAsync();
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if(personId==null)
            {
                return null;
            }

            Person? person = await _db. Persons.FirstOrDefaultAsync(p => p.PersonId == personId);

            if(person== null)
            {
                return null;
            }
            return person.ToPersonResponse();
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
            case nameof(PersonResponse.PersonName):
                matching=allPersons.Where(temp=>temp.PersonName != null && temp.PersonName!="" && 
                    temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case nameof(PersonResponse.Email):
                matching = allPersons.Where(temp => temp.Email != null && temp.Email!="" &&
                    temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case nameof(PersonResponse.DateOfBirth):
                matching = allPersons.Where(temp => temp.DateOfBirth != null &&
                    temp.DateOfBirth.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case nameof(PersonResponse.CountryID):
                matching = allPersons.Where(temp=> temp.Country != null && temp.Country != "" && 
                    temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
            case nameof(PersonResponse.Gender):
                matching = allPersons.Where(temp => temp.Gender != null &&
                    temp.Gender.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case nameof(PersonResponse.Address):
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

            Person? personResponse = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personUpdateRequest.PersonId);

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

            await _db.SaveChangesAsync();

            return personResponse.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);

            if (person == null)
            {
                return false;
            }

            _db.Persons.Remove(
                _db.Persons.First(temp=> temp.PersonId==personId));
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
