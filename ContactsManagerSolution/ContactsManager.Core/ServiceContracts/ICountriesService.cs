using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represent buisness logic for manipulating country entities.
    /// </summary>
    public interface ICountriesService
    {

        /// <summary>
        /// Adds a new country to the list of countries.
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Return Country Entity with newlky created one</returns>
        public Task<CountryResponse?> AddCountryAsync(CountryAddRequest countryAddRequest);


        /// <summary>
        /// Returns all Countries
        /// </summary>
        /// <returns>All countries from Country list as List</returns>
        public Task<List<CountryResponse>?> GetAllCountries();


        /// <summary>
        /// Return Country Object based on CountryID
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns>Counry response matching to that CountryID</returns>
        public Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);

        /// <summary>
        /// Add countries from Excel File
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns>Integer value of number of rows</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);

    }
}
