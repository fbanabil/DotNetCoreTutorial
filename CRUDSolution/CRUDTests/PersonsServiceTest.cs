using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using Xunit.Abstractions;
using AutoFixture;
using FluentAssertions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            var initialPersons = new List<Person>();
            var initialCountries = new List<Country>();

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Persons, initialPersons);
            dbContextMock.CreateDbSetMock(temp => temp.Countries, initialCountries);

            _countriesService = new CountriesService(dbContext);

            _personService = new PersonsService(dbContext, _countriesService);


            _testOutputHelper = testOutputHelper;
        }


        #region AddPerson

        [Fact]
        public async Task AddPerson_NullPerson()
        {
            PersonAddRequest? request = null;

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personService.AddPerson(request);
            //});

            Func<Task> action = async () =>
            {
               await _personService.AddPerson(request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            PersonAddRequest? request = _fixture.Build<PersonAddRequest>()
             .With(temp=>temp.PersonName,null as string)
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            Func<Task> action = async () =>
            {
                await _personService.AddPerson(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? request = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp=> temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            PersonResponse? personResponse = await _personService.AddPerson(request);



            //Assert.True(personResponse.PersonId != Guid.Empty);
            personResponse.PersonId.Should().NotBe(Guid.Empty);

            List<PersonResponse> personResponses = await _personService.GetAllPersons();

            //Assert.Contains(personResponse, personResponses);
            personResponses.Should().Contain(personResponse);
        }

        #endregion


        #region GetPersonByPersonId


        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            Guid? personId = null;

            PersonResponse? personResponse = await _personService.GetPersonByPersonId(personId);

            //Assert.Null(personResponse);
            personResponse.Should().BeNull();
        }



        [Fact]
        public async Task GetPersonByPersonId_ProperPersonId()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);

            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);

            PersonResponse? personResponseById = await _personService.GetPersonByPersonId(personResponse.PersonId);

            List<PersonResponse> personResponses = await _personService.GetAllPersons();

            //Assert
            //Assert.Equal(personResponse, personResponseById);
            personResponseById.Should().BeEquivalentTo(personResponse);
            //Assert.Contains(personResponseById, personResponses);
            personResponses.Should().Contain(personResponseById);
        }
        #endregion


        #region GettAllPersons

        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            List<PersonResponse> personResponses = await _personService.GetAllPersons();
            //Assert.Empty(personResponses);
            personResponses.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPersons_ProperList()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .With(temp=>temp.CountryID,countryResponse?.CountryID)
             .Create();

            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);


            countryAddRequest = _fixture.Create<CountryAddRequest>();

            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

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

            //Assert.Contains(personResponse, personResponses);
            personResponses.Should().Contain(personResponse);

            //Assert.Contains(personResponse2, personResponses);
            personResponses.Should().Contain(personResponse2);

        }

        #endregion


        #region GetFilteredPersons


        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();
            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);


            countryAddRequest = _fixture.Create<CountryAddRequest>();

            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();
            PersonResponse? personResponse2 = await _personService.AddPerson(personAddRequest);


            countryAddRequest = _fixture.Create<CountryAddRequest>();

            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();
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


            //Assert.Equal(3, personResponses.Count);
            personResponses.Should().HaveCount(3);

            foreach (var person in actual)
            {
                personResponses.Should().Contain(person);
            }
        }

        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);

            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone1@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .With(temp => temp.PersonName, "Nabil")
             .With(temp => temp.CountryID, countryResponse?.CountryID)
             .Create();

            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);


            countryAddRequest = _fixture.Create<CountryAddRequest>();

            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);

            personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone2@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .With(temp => temp.PersonName, "Natasha")
             .With(temp => temp.CountryID, countryResponse?.CountryID)
             .Create();

            PersonResponse? personResponse2 = await _personService.AddPerson(personAddRequest);


            countryAddRequest = _fixture.Create<CountryAddRequest>();

            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);

            personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone3@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .With(temp => temp.PersonName, "Abir")
             .With(temp => temp.CountryID, countryResponse?.CountryID)
             .Create();

            PersonResponse? personResponse3 = await _personService.AddPerson(personAddRequest);


            List<PersonResponse> actual = new List<PersonResponse>() { personResponse, personResponse2, personResponse3 };

            List<PersonResponse> personResponses = await _personService.GetFilteredPerson(nameof(Person.PersonName), "na");


            foreach (var person in actual)
            {
                if (person.PersonName != null)
                {
                    if (person.PersonName.Contains("na",StringComparison.OrdinalIgnoreCase))
                    {
                        //Assert.Contains(person, personResponses);
                        personResponses.Should().Contain(person);
                    }
                    else
                    {
                        //Assert.DoesNotContain(person, personResponses);
                        personResponses.Should().NotContain(person);
                    }
                }
            }
        }


        #endregion


        #region GetSortedPersons

        [Fact]
        public async Task GetSortedFunction()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();
            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);


            countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry2"
            };
            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();
            PersonResponse? personResponse2 = await _personService.AddPerson(personAddRequest);


            countryAddRequest = new CountryAddRequest()
            {
                CountryName = "TestCountry3"
            };
            countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();
            PersonResponse? personResponse3 = await _personService.AddPerson(personAddRequest);


            List<PersonResponse> actual = new List<PersonResponse>() { personResponse, personResponse2, personResponse3 };


            List<PersonResponse> personResponses = await _personService.GetSortedPersons(actual, nameof(Person.PersonName), SortOrderOptions.DESC);


            actual = actual.OrderByDescending(p => p.PersonName).ToList();

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
            //Assert.Equal(actual.Count, personResponses.Count);
            personResponses.Should().HaveCount(actual.Count);
            //for (int i = 0; i < actual.Count; i++)
            //{
            //    //Assert.Equal(actual[i], personResponses[i]);
            //    personResponses[i].Should().BeEquivalentTo(actual[i]);
            //}
            personResponses.Should().BeInDescendingOrder(p => p.PersonName);
        }


        #endregion


        #region UpdatePerson

        [Fact]

        public async Task UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? personUpdateRequest = null;
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personService.UpdatePerson(personUpdateRequest);
            //});

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid()
            };

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.UpdatePerson(personUpdateRequest);
            //});

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        // Person Name null
        public async Task UpdatePerson_NullPersonName()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);

            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.UpdatePerson(personUpdateRequest);
            //});
            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }



        [Fact]
        public async Task UpdatePerson_ProperUpdate()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);

            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Fahd";
            personUpdateRequest.Email = "nabil@gmail.com";

            PersonResponse? updated = await _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? GetById = await _personService.GetPersonByPersonId(personUpdateRequest.PersonId);

            //Assert.Equal(updated, GetById);
            GetById.Should().BeEquivalentTo(updated);
        }


        #endregion

        #region DeletePerson

        [Fact]
        public async Task DeletePerson_NullPersonId()
        {
            Guid? personId = null;
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personService.DeletePerson(personId);
            //});

            Func<Task> action = async () =>
            {
                await _personService.DeletePerson(personId);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            Guid? personId = Guid.NewGuid();
            
            bool isDeleted = await _personService.DeletePerson(personId);
            
            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeletePerson_ProperPersonId()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);
            
            bool isDeleted = await _personService.DeletePerson(personResponse.PersonId);

            List<PersonResponse> personResponses = await _personService.GetAllPersons();

            _testOutputHelper.WriteLine("Person Deleted : " + personResponse.ToString());
            _testOutputHelper.WriteLine("Persons Left : ");
            foreach (var person in personResponses)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
            //Assert.DoesNotContain(personResponse, personResponses);
            personResponses.Should().NotContain(personResponse);
        }
    #endregion
    }    
}
