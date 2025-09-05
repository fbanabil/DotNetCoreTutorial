using AutoFixture;
using Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsSorterService _personsSorterService;


        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        private readonly IPersonsRepository _personsRepository;
        private readonly ICountriesRepository _countriesRepository;

        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
 
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            var _diagonsticContext = new Mock<IDiagnosticContext>();
            var _loggerMock = new Mock<ILogger<PersonsGetterService>>();
            var _logger1Mock = new Mock<ILogger<PersonsAdderService>>();
            var _logger2Mock = new Mock<ILogger<PersonsDeleterService>>();
            var _logger3Mock = new Mock<ILogger<PersonsUpdaterService>>();
            var _logger4Mock = new Mock<ILogger<PersonsSorterService>>();

            _fixture = new Fixture();
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;

            _personsGetterService = new PersonsGetterService(_personsRepository, _loggerMock.Object, _diagonsticContext.Object);
            _personsAdderService = new PersonsAdderService(_personsRepository, _logger1Mock.Object, _diagonsticContext.Object);
            _personsDeleterService = new PersonsDeleterService(_personsRepository, _logger2Mock.Object, _diagonsticContext.Object);
            _personsUpdaterService = new PersonsUpdaterService(_personsRepository, _logger3Mock.Object, _diagonsticContext.Object);
            _personsSorterService = new PersonsSorterService(_personsRepository,_logger4Mock.Object, _diagonsticContext.Object);

            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository= _countriesRepositoryMock.Object;

            _countriesService = new CountriesService(_countriesRepository);


            _testOutputHelper = testOutputHelper;
        }


        #region AddPerson

        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            PersonAddRequest? request = null;

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personService.AddPerson(request);
            //});

            Func<Task> action = async () =>
            {
               await _personsAdderService.AddPerson(request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddPerson_NullPersonName_ToBeArgumentExeption()
        {
            PersonAddRequest? request = _fixture.Build<PersonAddRequest>()
             .With(temp=>temp.PersonName,null as string)
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            Func<Task> action = async () =>
            {
                await _personsAdderService.AddPerson(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails_ShouldBeSuccessfull()
        {
            PersonAddRequest? request = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp=> temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            Person person= request.ToPerson();
            PersonResponse expected = person.ToPersonResponse();

            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            PersonResponse? personResponse = await _personsAdderService.AddPerson(request);

            expected.PersonId= personResponse.PersonId;

            //Assert.True(personResponse.PersonId != Guid.Empty);
            personResponse.PersonId.Should().NotBe(Guid.Empty);
            personResponse.Should().Be(expected);
        }

        #endregion


        #region GetPersonByPersonId


        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            Guid? personId = null;

            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonId(personId);

            //Assert.Null(personResponse);
            personResponse.Should().BeNull();
        }



        [Fact]
        public async Task GetPersonByPersonId_ProperPersonId_ToBeSuccessful()
        {
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Someone@example.com")
             .With(temp => temp.DateOfBirth, DateTime.Parse("2000-01-01"))
             .Create();

            Person person=personAddRequest.ToPerson();

            PersonResponse expected = person.ToPersonResponse();

            _personsRepositoryMock.Setup(temp=>temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            PersonResponse got = await _personsGetterService.GetPersonByPersonId(person.PersonId);

            got.Should().NotBeNull();
            got.Should().Be(expected);

         
        
        }
        #endregion


        #region GettAllPersons

        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            _personsRepositoryMock.Setup(temp=>temp.GetAllPersons())
                .ReturnsAsync(new List<Person>());

            List<PersonResponse> personResponses = await _personsGetterService.GetAllPersons();
            //Assert.Empty(personResponses);
            personResponses.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPersons_ProperList_ToBeSuccessfull()
        {
            List<Person> people = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create(),

                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create(),

                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create()
            };

            List<PersonResponse> expected = people.Select(temp=> temp.ToPersonResponse()).ToList();

            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(people);

            List<PersonResponse> got = await _personsGetterService.GetAllPersons();
            

            got.Should().NotBeEmpty();
            got.Select(temp => expected.Should().Contain(temp));
        }

        #endregion


        #region GetFilteredPersons


        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            List<Person> people = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create(),

                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create(),

                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create()
            };

            List<PersonResponse> expected = people.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepositoryMock.Setup(temp=>temp.GetFilteredPersons(It.IsAny<Expression<Func<Person,bool>>>())).ReturnsAsync(people);

            List<PersonResponse> actual = await _personsGetterService.GetFilteredPerson(nameof(Person.PersonName), string.Empty);

            actual.Should().NotBeEmpty();
            actual.Select(temp => expected.Should().Contain(temp));
        }


        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
        {
            List<Person> people = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(temp=>temp.PersonName,"Fahad")
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create(),

                _fixture.Build<Person>()
                .With(temp=>temp.PersonName,"Nabil")
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create(),

                _fixture.Build<Person>()
                .With(temp=>temp.PersonName,"Fahim")
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create()
            };

            List<Person> expectedPersons= people.Where(temp => temp.PersonName.Contains("fa", StringComparison.OrdinalIgnoreCase)).ToList();

            List<PersonResponse> expected = expectedPersons.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(people);

            List<PersonResponse> actual = await _personsGetterService.GetFilteredPerson(nameof(Person.PersonName), "fa");

            actual.Should().NotBeEmpty();
            actual.Select(temp => expected.Should().Contain(temp));
        }


        #endregion


        #region GetSortedPersons

        [Fact]
        public async Task GetSortedFunction()
        {
            List<Person> people = new List<Person>()
            {
                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create(),

                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create(),

                _fixture.Build<Person>()
                .With(temp=>temp.Country,null as Country)
                .With(temp=> temp.Email,"some@gmail.com")
                .With(temp=>temp.DateOfBirth,DateTime.Parse("2002-01-01"))
                .Create()
            };

            List<PersonResponse> expected = people.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(people);

            List<PersonResponse> actual = await _personsSorterService.GetSortedPersons(expected, nameof(Person.PersonName),SortOrderOptions.DESC);

            expected =expected.OrderByDescending(temp => temp.PersonName).ToList();

            for(int i=0;i<actual.Count; i++)
            {
                actual[i].Should().Be(expected[i]);
            }

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
                await _personsUpdaterService.UpdatePerson(personUpdateRequest);
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
                await _personsUpdaterService.UpdatePerson(personUpdateRequest);
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

            PersonResponse? personResponse = await _personsAdderService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.UpdatePerson(personUpdateRequest);
            //});
            Func<Task> action = async () =>
            {
                await _personsUpdaterService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }



        [Fact]
        public async Task UpdatePerson_ProperUpdate()
        {
           Person person= _fixture.Build<Person>()
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Email, "some@example.com")
                .With(temp => temp.DateOfBirth, DateTime.Parse("2002-01-01"))
                .With(temp=>temp.Gender,GenderOptions.Male.ToString())
                .Create();


            Person updated_person= _fixture.Build<Person>()
                .With(temp => temp.PersonId, person.PersonId)
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Email, "somex@gmail.com")
                .With(temp => temp.Gender, GenderOptions.Male.ToString())
                .With(temp => temp.DateOfBirth, DateTime.Parse("2000-03-01"))
                .Create();
                
            PersonResponse expected = updated_person.ToPersonResponse();

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            _personsRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(updated_person);

            PersonUpdateRequest? personUpdateRequest = expected.ToPersonUpdateRequest();

            PersonResponse? actual = await _personsUpdaterService.UpdatePerson(personUpdateRequest);
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
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
                await _personsDeleterService.DeletePerson(personId);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            Guid? personId = Guid.NewGuid();
            
            bool isDeleted = await _personsDeleterService.DeletePerson(personId);
            
            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeletePerson_ProperPersonId()
        {
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Email, "some@example.com")
                .With(temp => temp.DateOfBirth, DateTime.Parse("2002-01-01"))
                .With(temp => temp.Gender, GenderOptions.Male.ToString())
                .Create();

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);
            _personsRepositoryMock.Setup(temp => temp.DeletePersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(true);
            bool isDeleted = await _personsDeleterService.DeletePerson(person.PersonId);
            isDeleted.Should().BeTrue();
        }
    #endregion
    }    
}
