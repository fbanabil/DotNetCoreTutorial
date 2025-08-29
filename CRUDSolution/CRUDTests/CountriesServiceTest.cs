using ServiceContracts;
using System;
using System.Collections.Generic;
using ServiceContracts.DTO;
using Entities;
using Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;
        private readonly Mock<ICountriesRepository> _countryRepositoryMock;
        private readonly ICountriesRepository _countryRepository;

        public CountriesServiceTest()
        {
            _fixture = new Fixture();
            
            _countryRepositoryMock=new Mock<ICountriesRepository>();
            
            _countryRepository = _countryRepositoryMock.Object;

            _countriesService = new CountriesService(_countryRepository);
        }


        #region AddCountry Tests


        // When CountryAddRequest is null, it should throw ArgumentNullException

        [Fact]
        public async void AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                // Act
                return _countriesService.AddCountryAsync(countryAddRequest);
            });
        }



        // When CountryName is null, it should throw ArgumentException

        [Fact]
        public async void AddCountry_CountryNameIsNull()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, null as string)
                .Create();

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(() =>
            //{
            //    // Act
            //    return _countriesService.AddCountryAsync(countryAddRequest);
            //});

            Func<Task> func = async () => await _countriesService.AddCountryAsync(countryAddRequest);   
            await func.Should().ThrowAsync<ArgumentException>();
        }


        // When CountryName is duplicate, it should throw ArgumentException

        [Fact]
        public async void AddCountry_DuplicateCountryNabe()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "DuplicateCountry")
                .Create();
            Country country = countryAddRequest.ToCountry();


            _countryRepositoryMock.Setup(repo => repo.GetCountryByCountryName("DuplicateCountry"))
                .ReturnsAsync(country);

            Func<Task> func = async () => await _countriesService.AddCountryAsync(countryAddRequest)
            ;
            await func.Should().ThrowAsync<ArgumentException>();
        }


        // When CountryName is valid, it should return a CountryResponse with the correct CountryID and CountryName

        [Fact]
        public async void AddCountry_ProperCountryDetails()
        {
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "ValidCountry")
                .Create();
            Country country = countryAddRequest.ToCountry();

            _countryRepositoryMock.Setup(repo => repo.GetCountryByCountryName("ValidCountry"))
                .ReturnsAsync((Country?)null);
            _countryRepositoryMock.Setup(repo => repo.Add(It.IsAny<Country>()))
                .ReturnsAsync(country);
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
            countryResponse?.CountryName.Should().Be(countryAddRequest.CountryName);
            countryResponse?.CountryID.Should().NotBe(Guid.Empty);
        }
        #endregion


        #region GetAllCountries Tests

        [Fact]

        //List of country should be empty by default(before adding any country)
        public async Task GettAllCountries_EmptyList()
        {
            // Arrange
            _countryRepositoryMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(new List<Country>());

            // Act

            List<CountryResponse>? countries = await _countriesService.GetAllCountries();
            // Assert
            //Assert.Empty(countries);
            countries.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>
            {
                _fixture.Create<CountryAddRequest>(),
                _fixture.Create<CountryAddRequest>(),
                _fixture.Create<CountryAddRequest>(),
                _fixture.Create<CountryAddRequest>()
            };

            List<Country> countries = new List<Country>();
            List<CountryResponse> expectedCountryResponses = new List<CountryResponse>();

            foreach (CountryAddRequest countryAddRequest in countryAddRequests)
            {
                _countryRepositoryMock.Setup(temp=> temp.GetCountryByCountryName(countryAddRequest.CountryName))
                    .ReturnsAsync((Country?)null);
                _countryRepositoryMock.Setup(temp => temp.Add(It.IsAny<Country>()))
                    .ReturnsAsync(countryAddRequest.ToCountry());
                CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);
                countries.Add(countryAddRequest.ToCountry());
                expectedCountryResponses.Add(countryResponse);
            }

            _countryRepositoryMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            // Act
            List<CountryResponse>? countryResponses = await _countriesService.GetAllCountries();
            // Assert
            
            countries.Select(temp=> temp.CountryName).Should().BeEquivalentTo(countryResponses.Select(temp => temp.CountryName));

        }

        #endregion

        #region GetCountryByCountryID Tests

        // Check if CountryID parameter is null, it should throw ArgumentNullException
        [Fact]
        public void GetCountryByCountryID_NullCountryID()
        {
            // Arrange
            Guid? countryID = null;
            // Assert
            //Assert.ThrowsAsync<ArgumentNullException>(() =>
            //{
            //    // Act
            //    return _countriesService.GetCountryByCountryID(countryID.Value);
            //});
            Func<Task> func = async () => await _countriesService.GetCountryByCountryID(countryID.Value);
            func.Should().ThrowAsync<ArgumentNullException>();
        }



        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            // Arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            Country country = countryAddRequest.ToCountry();
            // Act
            _countryRepositoryMock.Setup(repo => repo.GetCountryByCountryName(countryAddRequest.CountryName))
                .ReturnsAsync((Country?)null);
            _countryRepositoryMock.Setup(repo => repo.Add(It.IsAny<Country>()))
                .ReturnsAsync(country);

            CountryResponse? addedCountry = await _countriesService.AddCountryAsync(countryAddRequest);

            _countryRepositoryMock.Setup(repo => repo.GetCountryByCountryID(country.CountryID))
                .ReturnsAsync(country);
            CountryResponse? countryResponse = await _countriesService.GetCountryByCountryID(country.CountryID);

            // Assert
            //Assert.Equal(addedCountry, countryResponse);
            countryResponse.CountryName.Should().BeEquivalentTo(addedCountry.CountryName);
        }
        #endregion
    }
}