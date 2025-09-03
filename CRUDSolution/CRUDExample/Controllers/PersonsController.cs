using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilter;
using CRUDExample.Filters.ExeptionFilters;
using CRUDExample.Filters.ResourceFilter;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    [ResponseHeaderActionFilter("X-Custom-Key-From-Action", "Custom-Value",3)]
    [TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonsAlwaysRunResultFilter))]

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
        // [Route("/index")] // "/" override. it takes /index
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter),Order = 4)]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Key", "Custom-Value" ,1},Order =1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
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

            // Transfered to filer
            //ViewBag.SearchBy = searchBy;
            //ViewBag.SearchString = searchString;

            // Search Operation
            List<PersonResponse> persons = await _personsService.GetFilteredPerson(searchBy, searchString);



            // Transfered to filer
            //ViewBag.SortBy = sortBy;
            //ViewBag.SortOrder = sortOrder.ToString();
            
            
            // Sorting 
            List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, sortBy, sortOrder);

            return View(sortedPersons);
        }


        [Route("[action]")]
        [HttpGet]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Key-From-Action", "Custom-Value" ,1})]
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
        [TypeFilter(typeof(PersonCreatAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter),Arguments = new object[] {false})]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            PersonResponse personResponse = await _personsService.AddPerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }



        [HttpGet]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(TokenResultFilter))]
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
        [TypeFilter(typeof(PersonCreatAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {

            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personRequest.PersonId);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }


            PersonResponse updatedPerson = await _personsService.UpdatePerson(personRequest);
            return RedirectToAction("Index");
           
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
