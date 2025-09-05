using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using RepositoryContracts;
 
namespace Services
{
    public class CountriesService : ICountriesService
    {
        //Private readonly List<Country> _countries;

        private readonly ICountriesRepository _countriesRepository;

        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
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
            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException($"Country with name '{countryAddRequest.CountryName}' already exists.", nameof(countryAddRequest.CountryName));
            }

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            await _countriesRepository.Add(country);

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>?> GetAllCountries()
        {
            return (await _countriesRepository.GetAllCountries()).Select(temp => temp.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                throw new ArgumentNullException("CountryID can't be null");
            }

            Country? country = await _countriesRepository.GetCountryByCountryID(countryID.Value);
            if (country == null)
            {
                return null;
            }
            else return country.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream stream = new MemoryStream();

            await formFile.CopyToAsync(stream);
            int insertedRecords = 0;
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];
                int rowCount = worksheet.Dimension.Rows; 
                for (int row=2;row<=rowCount;row++)
                {
                    string? cellValue = Convert.ToString(worksheet.Cells[row, 1].Value);

                    if(!String.IsNullOrWhiteSpace(cellValue))
                    {
                        string? countryName = cellValue;

                        if(await _countriesRepository.GetCountryByCountryName(countryName) == null)
                        {
                            CountryAddRequest countryAddRequest = new CountryAddRequest()
                            {
                                CountryName=countryName
                            };
                            await AddCountryAsync(countryAddRequest);
                            insertedRecords++;
                        }
                    }
                }
            }
            return insertedRecords;
        }
    }
}
