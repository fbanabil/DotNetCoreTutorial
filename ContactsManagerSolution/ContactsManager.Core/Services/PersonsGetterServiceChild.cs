using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsGetterServiceChild : PersonsGetterService
    {

        public PersonsGetterServiceChild(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
            : base(personsRepository, logger, diagnosticContext)
        {
        }

        public async override Task<MemoryStream> GetPersonsExcel()
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
