using AutoFixture;
using Castle.Core.Logging;
using CRUDExample.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countryService;

        private readonly Mock<IPersonsService> _personsServiceMock;
        private readonly Mock<ICountriesService> _countriesServiceMock;

        private readonly IFixture _fixture;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;


        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<PersonsController>>();
            _personsServiceMock = new Mock<IPersonsService>();
            _countriesServiceMock = new Mock<ICountriesService>();
            _personsService = _personsServiceMock.Object;
            _countryService = _countriesServiceMock.Object;
        }

        #region index tests
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonList()
        {
            List<PersonResponse> person_response_list=_fixture.Create<List<PersonResponse>>();

            PersonsController personsController = new PersonsController(_personsService, _countryService,_loggerMock.Object);

            _personsServiceMock.Setup(temp=>temp.GetFilteredPerson(It.IsAny<string>(),It.IsAny<string>()))
                .ReturnsAsync(person_response_list);

            _personsServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(),It.IsAny<SortOrderOptions>()))
                .ReturnsAsync(person_response_list);

            IActionResult result = await personsController.Index(_fixture.Create<string>(),_fixture.Create<string>(),_fixture.Create<string>(),_fixture.Create<SortOrderOptions>());

            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();

            viewResult.ViewData.Model.Should().Be(person_response_list);

        }

        #endregion

        #region Create

        [Fact]
        public async Task Create_IfNoModelErrors_ReturnsToIndexView()
        {
            PersonAddRequest personsAddRequest = _fixture.Create<PersonAddRequest>();

            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countriesServiceMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

            PersonsController personsController = new PersonsController(_personsService, _countryService, _loggerMock.Object);


            IActionResult result = await personsController.Create(personsAddRequest);

            RedirectToActionResult viewResult = Assert.IsType<RedirectToActionResult>(result);

            viewResult.ActionName.Should().Be("Index");
            viewResult.ControllerName.Should().Be("Persons");
        }

        #endregion

    }
}
