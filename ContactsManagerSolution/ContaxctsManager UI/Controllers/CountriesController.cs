using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace Controllers
{
    public class CountriesController : Controller
    {

        private readonly ICountriesService _countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        [Route("[action]")] 
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage="Please select a valid Excel file";
                return View();
            }

            if(Path.GetExtension(excelFile.FileName).ToLower() != ".xlsx")
            {
                ViewBag.ErrorMessage = "Only .xlsx files are allowed";
                return View();
            }

            int rowsInserted = await _countriesService.UploadCountriesFromExcelFile(excelFile);

            ViewBag.Message = $"{rowsInserted} countries have been added successfully.";

            return View();
        }
    }
}
