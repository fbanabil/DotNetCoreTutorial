using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //Private readonly List<Country> _countries;

        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public async Task<CountryResponse?> AddCountryAsync(CountryAddRequest countryAddRequest)
        {
            // Validate the input
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            // Check if the country name is null or empty
            if (string.IsNullOrWhiteSpace(countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
            }

            // Check if the country already exists
            if (_countries.Where(temp=> temp.CountryName==countryAddRequest.CountryName).ToList().Count>0)
            {
                throw new ArgumentException($"Country with name '{countryAddRequest.CountryName}' already exists.", nameof(countryAddRequest.CountryName));
            }

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            _countries.Add(country);
             
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>?> GetAllCountries()
        {
            List<CountryResponse>? countryResponses = _countries.Select((country) => country.ToCountryResponse()).ToList();
            return countryResponses;
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if(countryID == null)
            {
                throw new ArgumentNullException("CountryID can't be null");
            }

            Country? country=_countries.FirstOrDefault(temp=> temp.CountryID==countryID);
            if (country == null)
            {
                return null;
            }
            else return country.ToCountryResponse();
        }
    }
}
