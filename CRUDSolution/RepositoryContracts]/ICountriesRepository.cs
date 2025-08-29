using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Country Entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds new Country to the data source
        /// </summary>
        /// <param name="country">Country Object</param>
        /// <returns>Country that added</returns>
        Task<Country> Add(Country country);

        /// <summary>
        /// Gets all Countries from the data source
        /// </summary>
        /// <returns>List of all Countries</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Gets Country by CountryID
        /// </summary>
        /// <param name="countryID">Guid of country</param>
        /// <returns>A country object matching the GUID</returns>
        Task<Country?> GetCountryByCountryID(Guid countryID);



        /// <summary>
        /// Search Country By Country Name
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>Country Object</returns>
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}
