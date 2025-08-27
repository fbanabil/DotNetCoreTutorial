using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //Private readonly List<Country> _countries;

        private readonly PersonsDbContext _db;

        public CountriesService(PersonsDbContext db)
        {
            _db = db;
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
            if (await _db.Countries.CountAsync(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException($"Country with name '{countryAddRequest.CountryName}' already exists.", nameof(countryAddRequest.CountryName));
            }

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            await _db.Countries.AddAsync(country);
            await _db.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>?> GetAllCountries()
        {
            List<CountryResponse>? countryResponses = await _db.Countries.Select((country) => country.ToCountryResponse()).ToListAsync();
            return countryResponses;
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                throw new ArgumentNullException("CountryID can't be null");
            }

            Country? country = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryID);
            if (country == null)
            {
                return null;
            }
            else return country.ToCountryResponse();
        }
    }
}
