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
    }
}
