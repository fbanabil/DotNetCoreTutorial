using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //Private readonly List<Country> _countries;

        private readonly ApplicationDbContext _db;

        public CountriesService(ApplicationDbContext db)
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

                        if(await _db.Countries.Where(temp=> temp.CountryName == countryName).CountAsync() == 0)
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
