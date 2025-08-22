using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
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
        public async Task<IActionResult> Index()
        {
            List<PersonResponse> persons = await _personsService.GetAllPersons();

            return View(persons);
        }
    }
}
