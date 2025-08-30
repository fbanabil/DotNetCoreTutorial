using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {

        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IPersonsService personsService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personsService = personsService;
            _countriesService = countriesService;
            _logger = logger;
        }


        [Route("[action]")] // Takes persons/index 
        // [Route("/index")] // "/" override.iT takes /index
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {

            _logger.LogInformation("Index action method of PersonsController called");

            _logger.LogDebug($"SearchBy: {searchBy}, SearchString: {searchString}, SortBy: {sortBy}, SortOrder: {sortOrder}");

            // Search Fields
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName)  , "Person Name" },
                { nameof(PersonResponse.Email)       , "Email" },
                { nameof(PersonResponse.DateOfBirth) , "Date of Birth" },
                { nameof(PersonResponse.Gender) , "Gender" },
                { nameof(PersonResponse.CountryID) , "Country" },
                { nameof(PersonResponse.Address)     , "Address" }
            };

            ViewBag.SearchBy = searchBy;
            ViewBag.SearchString = searchString;

            // Search Operation
            List<PersonResponse> persons = await _personsService.GetFilteredPerson(searchBy, searchString);

            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder.ToString();
            // Sorting 
            List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, sortBy, sortOrder);

            return View(sortedPersons);
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse>? countries =
                await _countriesService.GetAllCountries();

            ViewBag.Countries = countries?.Select(temp =>
            new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString()
            });


            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse>? countries =
                await _countriesService.GetAllCountries();

                ViewBag.Countries = countries;

                ViewBag.Countries = countries?.Select(temp =>
                new SelectListItem()
                {
                    Text = temp.CountryName,
                    Value = temp.CountryID.ToString()
                });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                //ViewBag.PersonName = personAddRequest.PersonName;

                return View(personAddRequest);
            }

            PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }


        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Edit(Guid? personId)
        {
            if (personId == null)
            {
                return RedirectToAction("Index");
            }
            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personId);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse>? countries =
               await _countriesService.GetAllCountries();

            ViewBag.Countries = countries;

            ViewBag.Countries = countries?.Select(temp =>
            new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString()
            });

            return View(personUpdateRequest);
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {

            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = await _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                PersonUpdateRequest personUpdateRequest_1 = personResponse.ToPersonUpdateRequest();

                List<CountryResponse>? countries =
                   await _countriesService.GetAllCountries();

                ViewBag.Countries = countries;

                ViewBag.Countries = countries?.Select(temp =>
                new SelectListItem()
                {
                    Text = temp.CountryName,
                    Value = temp.CountryID.ToString()
                });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View(personUpdateRequest_1);
            }
        }


        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(Guid? personId)
        {
            if (personId == null)
            {
                return RedirectToAction("Index");
            }

            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personId);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete(PersonResponse personResponse)
        {
            PersonResponse? existingPerson = await _personsService.GetPersonByPersonId(personResponse.PersonId);
            if (existingPerson == null)
            {
                return RedirectToAction("Index");
            }
            bool deleteStatus = await _personsService.DeletePerson(personResponse.PersonId);
            return RedirectToAction("Index");
        }

        //Pdf View
        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> personResponses = await _personsService.GetAllPersons();

            //Return View as PDF

            return new ViewAsPdf("PersonsPDF", personResponses, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins { Top = 10, Bottom = 10, Left = 10, Right = 10
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]         
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream  memoryStream = await _personsService.GetPersonCSV();
            return File(memoryStream, "application/octet-stream", "Persons.csv");
        }


        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Persons.xlsx");
        }
    }
}
