using ServiceContracts;
using System;
using System.Collections.Generic;
using ServiceContracts.DTO;
using Entities;
using Services;
namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }


        #region AddCountry Tests


        // When CountryAddRequest is null, it should throw ArgumentNullException

        [Fact]
        public async void AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(()=>
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
            CountryAddRequest? countryAddRequest = new CountryAddRequest
            {
                CountryName = null
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                // Act
                 return _countriesService.AddCountryAsync(countryAddRequest);
            });
        }


        // When CountryName is duplicate, it should throw ArgumentException

        [Fact]
        public async void AddCountry_DuplicateCountryNabe()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest
            {
                CountryName = "DuplicateCountry"
            };

            CountryAddRequest duplicateCountryAddRequest = new CountryAddRequest
            {
                CountryName = "DuplicateCountry"
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountryAsync(countryAddRequest);
                return _countriesService.AddCountryAsync(duplicateCountryAddRequest);
            });
        }


        // When CountryName is valid, it should return a CountryResponse with the correct CountryID and CountryName

        [Fact]
        public async void AddCountry_ProperCountryDetails()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest
            {
                CountryName = "ValidCountry"
            };

            // Act
            CountryResponse? countryResponse =await _countriesService.AddCountryAsync(countryAddRequest);

            List<CountryResponse>? allCountries = await _countriesService.GetAllCountries();
            // Assert

            Assert.True(countryResponse.CountryID != Guid.Empty, "CountryID should not be empty");
            Assert.Contains(countryResponse,allCountries);
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
            Assert.Empty(countries);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>
            {
                new CountryAddRequest { CountryName = "Country1" },
                new CountryAddRequest { CountryName = "Country2" },
                new CountryAddRequest { CountryName = "Country3" }
            };

            List<CountryResponse>? expectedCountries = new List<CountryResponse>();
            //Act
            foreach (CountryAddRequest countryAddRequest in countryAddRequests)
            {
                expectedCountries.Add(await _countriesService.AddCountryAsync(countryAddRequest));
            }

            List<CountryResponse>? countries = await _countriesService.GetAllCountries();

            // Assert
            foreach(CountryResponse expected_country in expectedCountries)
            {
                Assert.Contains(expected_country,countries);
            }

        }

        #endregion
    }
}
