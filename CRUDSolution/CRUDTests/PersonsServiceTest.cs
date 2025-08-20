using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }


        #region AddPerson

        [Fact]
        public async Task AddPerson_NullPerson()
        {
            PersonAddRequest? request = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.AddPerson(request);
            });
        }

        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            PersonAddRequest? request = new PersonAddRequest()
            {
                PersonName = null
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.AddPerson(request);
            });
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? request = new PersonAddRequest()
            {
                PersonName = "TestName",
                Email = "Test@gmail.com",
                Address = "Address",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                RecieveNewsLetters = true,
            };

            PersonResponse? personResponse = await _personService.AddPerson(request);

            Assert.True(personResponse.PersonId != Guid.Empty);

            List<PersonResponse> personResponses = await _personService.GetAllPersons();

            Assert.Contains(personResponse, personResponses);
        }

        #endregion


        #region GetPersonByPersonId


        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            Guid? personId = null;

            PersonResponse? personResponse = await _personService.GetPersonByPersonId(personId);

            Assert.Null(personResponse);
        }



        [Fact]
        public async Task GetPersonByPersonId_ProperPersonId()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry"
            };

            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);

            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "TestPerson",
                Email = "TestEmail@gmail.com",
                Address = "TestAddress",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Female,
                RecieveNewsLetters = false
            };

            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);

            PersonResponse? personResponseById = await _personService.GetPersonByPersonId(personResponse.PersonId);

            List<PersonResponse> personResponses = await _personService.GetAllPersons();

            //Assert
            Assert.Equal(personResponse, personResponseById);
            Assert.Contains(personResponseById, personResponses);
        }
        #endregion


        #region GettAllPersons

        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            List<PersonResponse> personResponses = await _personService.GetAllPersons();
            Assert.Empty(personResponses);
        }

        [Fact]
        public async Task GetAllPersons_ProperList()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry"
            };
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "TestPerson",
                Email = "Test@gmail.com",
                Address = "TestAddress",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Male,
                RecieveNewsLetters = true
            };
            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);


            countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry2"
            };
            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = new PersonAddRequest()
            {
                PersonName = "TestPerson2",
                Email = "x@gmail.com",
                Address = "TestAddress2",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Male,
                RecieveNewsLetters = true
            };
            PersonResponse? personResponse2 = await _personService.AddPerson(personAddRequest);

            List<PersonResponse> actual = new List<PersonResponse>();
            actual.Add(personResponse);
            actual.Add(personResponse2);

            List<PersonResponse> personResponses = await _personService.GetAllPersons();

            _testOutputHelper.WriteLine("Expected Output : ");
            foreach (var person in actual)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _testOutputHelper.WriteLine("Actual Output : ");
            foreach (var person in personResponses) 
            {
                _testOutputHelper.WriteLine(person.ToString());
            }


            Assert.Equal(2, personResponses.Count);

            Assert.Contains(personResponse, personResponses);
            Assert.Contains(personResponse2, personResponses);

        }

        #endregion


        #region GetFilteredPersons


        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry"
            };
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "TestPerson",
                Email = "Test@gmail.com",
                Address = "TestAddress",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Male,
                RecieveNewsLetters = true
            };
            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);


            countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry2"
            };
            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = new PersonAddRequest()
            {
                PersonName = "TestPerson2",
                Email = "x@gmail.com",
                Address = "TestAddress2",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Male,
                RecieveNewsLetters = true
            };
            PersonResponse? personResponse2 = await _personService.AddPerson(personAddRequest);
            

            countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry3"
            };
            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = new PersonAddRequest()
            {
                PersonName = "TestPerson3",
                Email = "Y@gmail.com",
                Address = "TestAddress3",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Male,
                RecieveNewsLetters = true
            };
            PersonResponse? personResponse3 = await _personService.AddPerson(personAddRequest);


            List<PersonResponse> actual = new List<PersonResponse>() { personResponse, personResponse2, personResponse3 };

            List<PersonResponse> personResponses = await _personService.GetFilteredPerson(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Expected Output : ");
            foreach (var person in actual)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _testOutputHelper.WriteLine("Actual Output : ");
            foreach (var person in personResponses)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }


            Assert.Equal(3, personResponses.Count);
            
            foreach (var person in actual)
            {
                Assert.Contains(person, personResponses);
            }
        }

        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry"
            };
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "TestPerson",
                Email = "Test@gmail.com",
                Address = "TestAddress",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Male,
                RecieveNewsLetters = true
            };
            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);


            countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry2"
            };
            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = new PersonAddRequest()
            {
                PersonName = "TestPerson2",
                Email = "x@gmail.com",
                Address = "TestAddress2",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Male,
                RecieveNewsLetters = true
            };
            PersonResponse? personResponse2 = await _personService.AddPerson(personAddRequest);


            countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry3"
            };
            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = new PersonAddRequest()
            {
                PersonName = "ThirdPerson",
                Email = "Y@gmail.com",
                Address = "TestAddress3",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-04-11"),
                Gender = GenderOptions.Male,
                RecieveNewsLetters = true
            };
            PersonResponse? personResponse3 = await _personService.AddPerson(personAddRequest);


            List<PersonResponse> actual = new List<PersonResponse>() { personResponse, personResponse2, personResponse3 };

            List<PersonResponse> personResponses = await _personService.GetFilteredPerson(nameof(Person.PersonName), "te");


            foreach (var person in actual)
            {
                if (person.PersonName != null)
                {
                    if (person.PersonName.ToLower().Contains("te"))
                    {
                        Assert.Contains(person, personResponses);
                    }
                    else
                    {
                        Assert.DoesNotContain(person, personResponses);
                    }
                }
            }
        }


        #endregion

    }
}
