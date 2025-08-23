using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Security;
using System.Threading.Tasks;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {

        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonsService personsService,ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }


        [Route("[action]")] // Takes persons/index 
        // [Route("/index")] // "/" override.iT takes /index
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy,string? searchString,
            string sortBy=nameof(PersonResponse.PersonName) , 
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
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

            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem()
            {
                Text=temp.CountryName,Value=temp.CountryID.ToString()
            });
            

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if(!ModelState.IsValid)
            {
                List<CountryResponse>? countries =
                await _countriesService.GetAllCountries();

                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.PersonName = personAddRequest.PersonName;

                return View();
            }

            PersonResponse personResponse= await _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }
    }
}
