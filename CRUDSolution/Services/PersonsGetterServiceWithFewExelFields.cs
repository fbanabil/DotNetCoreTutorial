using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsGetterServiceWithFewExelFields : IPersonsGetterService
    {

        private readonly PersonsGetterService _personsGetterService;
        public PersonsGetterServiceWithFewExelFields(PersonsGetterService personsGetterService)
        {
            _personsGetterService = personsGetterService;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personsGetterService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
        {
            return await _personsGetterService.GetFilteredPerson(searchBy, searchString);
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            return await _personsGetterService.GetPersonByPersonId(personId);
        }

        public async Task<MemoryStream> GetPersonCSV()
        {
            return await _personsGetterService.GetPersonCSV();
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

                using (ExcelRange headerCells = worksheet.Cells["A1:C1"])
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
                    if (personResponse.DateOfBirth != null)
                    {
                        worksheet.Cells[row, column++].Value = personResponse.DateOfBirth?.ToString("yyyy-MMM-dd");
                    }
                    else worksheet.Cells[row, column++].Value = "";
                    row++;
                    column = 1;
                }

                worksheet.Cells[$"A1:C{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
