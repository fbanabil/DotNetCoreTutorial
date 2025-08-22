using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Security;

namespace CRUDExample.Controllers
{
    public class PersonsController : Controller
    {

        private readonly IPersonsService _personsService;

        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }


        [Route("persons/index")]
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
    }
}
