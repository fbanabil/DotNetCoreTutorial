using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //Private readonly List<Country> _countries;

        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                // {73ECD269-285B-42CA-9BB3-FA288B5C2299}
                _countries.AddRange(new List<Country>()
                {
                    new Country()
                    {
                        CountryID = Guid.Parse("73ECD269-285B-42CA-9BB3-FA288B5C2299"),
                        CountryName = "India"
                    },

                    // {6C6AFC4D-516C-45E6-8D7B-94AD4591FB4E}
                    new Country()
                    {
                        CountryID = Guid.Parse("6C6AFC4D-516C-45E6-8D7B-94AD4591FB4E"),
                        CountryName = "USA"
                    },
                    // {390CF153-CD9C-4B8D-8A8F-1ECBB299C3F3}
                    new Country()
                    {
                        CountryID = Guid.Parse("390CF153-CD9C-4B8D-8A8F-1ECBB299C3F3"),
                        CountryName = "UK"
                    },
                    // {BAADAA75-4FE4-46D3-AB45-21EFA840122C}
                    new Country()
                    {
                        CountryID = Guid.Parse("BAADAA75-4FE4-46D3-AB45-21EFA840122C"),
                        CountryName = "Australia"
                    },
                    // {4A146B3D-9F92-42FB-9FDF-1942C9850C39}
                    new Country()
                    {
                        CountryID = Guid.Parse("4A146B3D-9F92-42FB-9FDF-1942C9850C39"),
                        CountryName = "Canada"
                    }
                });

            }

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
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).ToList().Count > 0)
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
            if (countryID == null)
            {
                throw new ArgumentNullException("CountryID can't be null");
            }

            Country? country = _countries.FirstOrDefault(temp => temp.CountryID == countryID);
            if (country == null)
            {
                return null;
            }
            else return country.ToCountryResponse();
        }
    }
}
