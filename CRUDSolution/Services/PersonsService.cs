using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;


namespace Services
{
    public class PersonsService : IPersonsService
    {

        private readonly List<Person>? _persons;
        private readonly ICountriesService _countriesService;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsService(bool initialize = true)
        {
            _countriesService = new CountriesService();
            _persons = new List<Person>();

            if (initialize)
            {
                //{A3D4D4D1-C2D9-4D85-A36E-DAC4290F1FB5}
                //{B188B176-8A8F-4BB5-A09B-FD1610FE4738}
                //{0D7AA664-E86F-4E76-AFC4-781EA2465171}
                //{193F65FF-73CF-4CD3-AE11-1A2574E5B167}
                //{4BF6A56B-3B91-4DBC-A0F2-0295642CC066}
                //{AC94C524-2E28-4CAE-87F6-C4F458D0CE6D}
                //{997CEB1B-A523-41D6-A272-D6F6EFB2BEB5}
                //{63B2CA17-6E7A-47B6-80D6-F783BA869517}
                //{7B672DF6-7065-4832-B849-2F1CAFC56E71}
                //{06569EE6-F490-4C0A-B572-1DDCBAB2D8A9}

                /*
                 * Archibold,aetheredge0@imgur.com,4/27/1989,Male,708 Lindbergh Place,true
                Duffie,dworsfield1@ycombinator.com,5/3/1981,Male,20627 Oriole Junction,true
                Ruy,rmichele2@wikia.com,4/10/1997,Male,2 Sunnyside Center,true
                Tobi,tjanjusevic3@ebay.co.uk,4/1/1986,Female,00110 Northwestern Junction,false
                Anjela,avaneev4@naver.com,1/23/1982,Female,8 Heffernan Center,true
                Boot,bstruys5@senate.gov,12/6/2006,Male,0711 Maryland Court,true
                Kahlil,kfaley6@nba.com,7/17/1995,Male,88 Glacier Hill Park,false
                Gilbertina,gjanda7@wordpress.org,4/22/2012,Female,7111 Lillian Parkway,true
                Costa,chawgood8@wikimedia.org,6/22/1985,Male,979 Cascade Place,true
                Claude,cmalpas9@csmonitor.com,4/7/2006,Female,4395 Pond Circle,true
                */
                _persons.AddRange(new List<Person>()
                {
                    new Person()
                    {
                        PersonName="Archibold",
                        Email="aetheredge0@imgur.com",
                        DateOfBirth=DateTime.Parse("4/27/1989"),
                        Gender="Male",
                        Address="708 Lindbergh Place",
                        RecieveNewsLetters = true,
                        PersonId = Guid.Parse("A3D4D4D1-C2D9-4D85-A36E-DAC4290F1FB5"),
                        CountryID = Guid.Parse("73ECD269-285B-42CA-9BB3-FA288B5C2299")
                    },
                    new Person()
                    {
                        PersonName="Duffie",
                        Email="dworsfield1@ycombinator.com",
                        DateOfBirth=DateTime.Parse("5/3/1981"),
                        Gender="Male",
                        Address = "20627 Oriole Junction",
                        RecieveNewsLetters = true,
                        PersonId = Guid.Parse("B188B176-8A8F-4BB5-A09B-FD1610FE4738"),
                        CountryID = Guid.Parse("6C6AFC4D-516C-45E6-8D7B-94AD4591FB4E")
                    },
                    new Person()
                    {
                        PersonName="Ruy",
                        Email="rmichele2@wikia.com",
                        DateOfBirth=DateTime.Parse("4/10/1997"),
                        Gender="Male",
                        Address = "2 Sunnyside Center",
                        RecieveNewsLetters = true,
                        PersonId = Guid.Parse("0D7AA664-E86F-4E76-AFC4-781EA2465171"),
                        CountryID = Guid.Parse("390CF153-CD9C-4B8D-8A8F-1ECBB299C3F3")
                    },
                    new Person()
                    {
                        PersonName="Tobi",
                        Email="tjanjusevic3@ebay.co.uk",
                        DateOfBirth=DateTime.Parse("4/1/1986"),
                        Gender = "Female",
                        Address = "00110 Northwestern Junction",
                        RecieveNewsLetters = false,
                        PersonId = Guid.Parse("193F65FF-73CF-4CD3-AE11-1A2574E5B167"),
                        CountryID = Guid.Parse("BAADAA75-4FE4-46D3-AB45-21EFA840122C")
                    },
                    new Person()
                    {
                        PersonName="Anjela",
                        Email="avaneev4@naver.com",
                        DateOfBirth=DateTime.Parse("1/23/1982"),
                        Gender = "Female",
                        Address = "8 Heffernan Center",
                        RecieveNewsLetters = false,
                        PersonId = Guid.Parse("4BF6A56B-3B91-4DBC-A0F2-0295642CC066"),
                        CountryID = Guid.Parse("4A146B3D-9F92-42FB-9FDF-1942C9850C39")
                    }
                    ,
                    new Person()
                    {
                        PersonName="Boot",
                        Email="bstruys5@senate.gov",
                        DateOfBirth=DateTime.Parse("12/6/2006"),
                        Gender="Male",
                        Address = "0711 Maryland Court",
                        RecieveNewsLetters = true,
                        PersonId = Guid.Parse("AC94C524-2E28-4CAE-87F6-C4F458D0CE6D"),
                        CountryID = Guid.Parse("4A146B3D-9F92-42FB-9FDF-1942C9850C39")
                    },
                    new Person()
                    {
                        PersonName="Kahlil",
                        Email="kfaley6@nba.com",
                        DateOfBirth=DateTime.Parse("7/17/1995"),
                        Gender="Male",
                        Address = "88 Glacier Hill Park",
                        RecieveNewsLetters = false,
                        PersonId = Guid.Parse("997CEB1B-A523-41D6-A272-D6F6EFB2BEB5"),
                        CountryID = Guid.Parse("6C6AFC4D-516C-45E6-8D7B-94AD4591FB4E")
                    },
                    new Person()
                    {
                        PersonName="Gilbertina",
                        Email="gjanda7@wordpress.org",
                        DateOfBirth=DateTime.Parse("4/22/2012"),
                        Gender = "Female",
                        Address = "7111 Lillian Parkway",
                        RecieveNewsLetters = true,
                        PersonId = Guid.Parse("63B2CA17-6E7A-47B6-80D6-F783BA869517"),
                        CountryID = Guid.Parse("BAADAA75-4FE4-46D3-AB45-21EFA840122C")
                    },
                    new Person()
                    {
                        PersonName="Costa",
                        Email="chawgood8@wikimedia.org",
                        DateOfBirth=DateTime.Parse("6/22/1985"),
                        Gender = "Male",
                        Address = "979 Cascade Place",
                        RecieveNewsLetters = true,
                        PersonId = Guid.Parse("7B672DF6-7065-4832-B849-2F1CAFC56E71"),
                        CountryID = Guid.Parse("390CF153-CD9C-4B8D-8A8F-1ECBB299C3F3")
                    },
                    new Person()
                    {
                        PersonName="Claude",
                        Email="cmalpas9@csmonitor.com",
                        DateOfBirth = DateTime.Parse("4/7/2006"),
                        Gender = "Female",
                        Address = "4395 Pond Circle",
                        RecieveNewsLetters = true,
                        PersonId = Guid.Parse("06569EE6-F490-4C0A-B572-1DDCBAB2D8A9"),
                        CountryID = Guid.Parse("6C6AFC4D-516C-45E6-8D7B-94AD4591FB4E")
                    }

                });
            }
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
            if (personId == null)
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
