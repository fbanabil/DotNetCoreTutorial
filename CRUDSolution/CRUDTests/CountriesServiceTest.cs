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

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture; 
        public CountriesServiceTest()
        {
            _fixture = new Fixture();
            var countriesInitial = new List<Country>();

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object;  
            
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitial);


            _countriesService = new CountriesService(dbContext);
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
            CountryAddRequest countryAddRequest = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "DuplicateCountry")
                .Create();

            CountryAddRequest duplicateCountryAddRequest = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "DuplicateCountry")
                .Create();

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(() =>
            //{
            //    // Act
            //    _countriesService.AddCountryAsync(countryAddRequest);
            //    return _countriesService.AddCountryAsync(duplicateCountryAddRequest);
            //});
            await _countriesService.AddCountryAsync(countryAddRequest); 
            Func<Task> func = async () => await _countriesService.AddCountryAsync(duplicateCountryAddRequest);
            await func.Should().ThrowAsync<ArgumentException>();
        }


        // When CountryName is valid, it should return a CountryResponse with the correct CountryID and CountryName

        [Fact]
        public async void AddCountry_ProperCountryDetails()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();

            // Act
            CountryResponse? countryResponse = await _countriesService.AddCountryAsync(countryAddRequest);

            List<CountryResponse>? allCountries = await _countriesService.GetAllCountries();
            // Assert

            //Assert.True(countryResponse.CountryID != Guid.Empty, "CountryID should not be empty");

            countryResponse?.CountryID.Should().NotBe(Guid.Empty,"Country Is Should Not Be Empty");

            //Assert.Contains(countryResponse, allCountries);
            allCountries.Should().Contain(countryResponse);
        }
        #endregion


        #region GetAllCountries Tests

        [Fact]

        //List of country should be empty by default(before adding any country)
        public async Task GettAllCountries_EmptyList()
        {
            // Arrange
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

            List<CountryResponse>? expectedCountries = new List<CountryResponse>();
            //Act
            foreach (CountryAddRequest countryAddRequest in countryAddRequests)
            {
                expectedCountries.Add(await _countriesService.AddCountryAsync(countryAddRequest));
            }

            List<CountryResponse>? countries = await _countriesService.GetAllCountries();

            // Assert
            foreach (CountryResponse expected_country in expectedCountries)
            {
                //Assert.Contains(expected_country, countries);
                countries.Should().Contain(expected_country);
            }

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
            // Act
            CountryResponse? addedCountry = await _countriesService.AddCountryAsync(countryAddRequest);
            CountryResponse? countryResponse = await _countriesService.GetCountryByCountryID(addedCountry.CountryID);

            // Assert
            //Assert.Equal(addedCountry, countryResponse);
            countryResponse.Should().BeEquivalentTo(addedCountry);
        }
        #endregion
    }
}