using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsGetterService : IPersonsGetterService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsGetterService(IPersonsRepository personsRepository,ILogger<PersonsGetterService> logger,IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            _personsRepository = personsRepository;
        }
    


        public virtual async Task<List<PersonResponse>> GetAllPersons()
        {
            _logger.LogInformation("GetAllPersons method of PersonsService called");
            return (await _personsRepository.GetAllPersons())
                .Select(person => person.ToPersonResponse()).ToList();
        }

        public virtual async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if(personId==null)
            {
                return null;
            }

            Person? person = await _personsRepository.GetPersonByPersonID(personId.Value);

            if (person== null)
            {
                return null;
            }
            return person.ToPersonResponse();
        }

        public virtual async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
        {
            _logger.LogInformation("GetFilteredPerson method of PersonsService called");

            if (searchString==null)
            {
                return await GetAllPersons();
            }
            List<Person> matching=new List<Person>();   
            //Serilog timing
            using (Operation.Time("Time for filtered person from database"))
            {

                matching = searchBy switch
                {
                    nameof(PersonResponse.PersonName) =>
                     await _personsRepository.GetFilteredPersons(temp => temp.PersonName.Contains(searchString)),

                    nameof(PersonResponse.Email) =>
                         await _personsRepository.GetFilteredPersons(temp => temp.Email.Contains(searchString)),

                    nameof(PersonResponse.DateOfBirth) =>
                         await _personsRepository.GetFilteredPersons(temp => temp.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString)),

                    nameof(PersonResponse.CountryID) =>
                         await _personsRepository.GetFilteredPersons(temp => temp.Country.CountryName.Contains(searchString)),


                    nameof(PersonResponse.Gender) =>
                         await _personsRepository.GetFilteredPersons(temp => temp.Gender.Contains(searchString)),

                    nameof(PersonResponse.Address) =>
                         await _personsRepository.GetFilteredPersons(temp => temp.Address.Contains(searchString)),

                    _ => await _personsRepository.GetAllPersons()

                };
            }
            _diagnosticContext.Set("Persons", matching);
            return matching.Select(temp => temp.ToPersonResponse()).ToList();
        }

    
        public virtual async Task<MemoryStream> GetPersonCSV()
        {
            MemoryStream memoryStream = new MemoryStream();

            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvConfiguration csvConfiguration = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);

            //CsvWriter csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture,leaveOpen : true);
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);


            //csvWriter.WriteHeader<PersonResponse>();//Property namea as header rows
            
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.RecieveNewsLetters));

            csvWriter.NextRecord();

            List<PersonResponse>? personsResponses = await GetAllPersons();

            //await csvWriter.WriteRecordsAsync(personsResponses);

            foreach(PersonResponse response in personsResponses)
            {
                csvWriter.WriteField(response.PersonName);
                csvWriter.WriteField(response.Email);
                if (response.DateOfBirth != null)
                {
                    csvWriter.WriteField(response.DateOfBirth?.ToString("yyyy-MMM-dd"));
                }
                else csvWriter.WriteField("");
                csvWriter.WriteField(response.Age);
                csvWriter.WriteField(response.Gender);
                csvWriter.WriteField(response.Address);
                csvWriter.WriteField(response.RecieveNewsLetters);
                csvWriter.NextRecord();
                csvWriter.Flush();
            }
            
            memoryStream.Position = 0;

            return memoryStream;
        }

        public virtual async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsWorksheet");

                worksheet.Cells["A1"].Value = nameof(PersonResponse.PersonName);
                worksheet.Cells["B1"].Value = nameof(PersonResponse.Email);
                worksheet.Cells["C1"].Value = nameof(PersonResponse.DateOfBirth);
                worksheet.Cells["D1"].Value = nameof(PersonResponse.Age);
                worksheet.Cells["E1"].Value = nameof(PersonResponse.Gender);
                worksheet.Cells["F1"].Value = nameof(PersonResponse.Country);
                worksheet.Cells["G1"].Value = nameof(PersonResponse.Address);   
                worksheet.Cells["H1"].Value = nameof(PersonResponse.RecieveNewsLetters);

                using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Font.Bold = true;
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                List<PersonResponse>? personsResponses = await GetAllPersons();

                int row = 2;
                int column = 1;
                foreach (PersonResponse personResponse in personsResponses)
                {
                    worksheet.Cells[row, column++].Value = personResponse.PersonName;   
                    worksheet.Cells[row, column++].Value = personResponse.Email;
                    if(personResponse.DateOfBirth != null)
                    {
                        worksheet.Cells[row, column++].Value = personResponse.DateOfBirth?.ToString("yyyy-MMM-dd");
                    }
                    else worksheet.Cells[row, column++].Value = "";
                    worksheet.Cells[row, column++].Value = personResponse.Age;
                    worksheet.Cells[row, column++].Value = personResponse.Gender;
                    worksheet.Cells[row, column++].Value = personResponse.Country;
                    worksheet.Cells[row, column++].Value = personResponse.Address;
                    worksheet.Cells[row, column++].Value = personResponse.RecieveNewsLetters;
                    row++;
                    column = 1;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
