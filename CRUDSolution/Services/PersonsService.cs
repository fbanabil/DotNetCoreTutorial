using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
using Microsoft.EntityFrameworkCore;
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
    public class PersonsService : IPersonsService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsService(IPersonsRepository personsRepository,ILogger<PersonsService> logger,IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            _personsRepository = personsRepository;
        }
    


        //Person to person resoponse method

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //Null argument
            if(personAddRequest==null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Null Person name
            if(string.IsNullOrEmpty(personAddRequest.PersonName))
            {
                throw new ArgumentException("Person can't be blank");
            }

            //Model Validation
            ValidationHelper.ModelValidate(personAddRequest);


            // Create person id
            Person person = personAddRequest.ToPerson();

            //Assign Guid
            person.PersonId = Guid.NewGuid();

            //Add person
            await _personsRepository.AddPerson(person);
            //await _db.SaveChangesAsync();

            //_db.sp_InsertPerson(person);

            //Return person
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            _logger.LogInformation("GetAllPersons method of PersonsService called");
            return (await _personsRepository.GetAllPersons())
                .Select(person => person.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
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

        public async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
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

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> personResponses, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedPersons method of PersonsService called");

            if (string.IsNullOrEmpty(sortBy) || personResponses == null || personResponses.Count == 0)
            {
                return personResponses;
            }

            // Sort the personResponses based on the sortBy parameter

            List<PersonResponse> sortedResponse = (sortBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.PersonName,StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) 
                    => personResponses.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),


                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Age).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.RecieveNewsLetters).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.RecieveNewsLetters).ToList(),

                _=> personResponses
            };

            return sortedResponse;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest==null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            if(personUpdateRequest.PersonName==null)
            {
                throw new ArgumentException(nameof(personUpdateRequest.PersonName));
            }

            ValidationHelper.ModelValidate(personUpdateRequest);

            Person? personResponse = await _personsRepository.GetPersonByPersonID(personUpdateRequest.PersonId);

            if (personResponse==null)
            {
                throw new InvalidPersonIDException("Given PersonID doesn't exist");
            }


            personResponse.PersonName = personUpdateRequest.PersonName;
            personResponse.Email = personUpdateRequest.Email;
            personResponse.DateOfBirth = personUpdateRequest.DateOfBirth;
            personResponse.Gender = personUpdateRequest.Gender.ToString();
            personResponse.CountryID = personUpdateRequest.CountryID;
            personResponse.Address = personUpdateRequest.Address;
            personResponse.RecieveNewsLetters = personUpdateRequest.RecieveNewsLetters;

            await _personsRepository.UpdatePerson(personResponse);

            return personResponse.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = await _personsRepository.GetPersonByPersonID(personId.Value);

            if (person == null)
            {
                return false;
            }

            return await _personsRepository.DeletePersonByPersonId(personId.Value);
        }

        public async Task<MemoryStream> GetPersonCSV()
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

        public async Task<MemoryStream> GetPersonsExcel()
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
