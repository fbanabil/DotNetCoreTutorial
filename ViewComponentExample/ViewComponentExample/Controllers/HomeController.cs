using Microsoft.AspNetCore.Mvc;
using ViewComponentExample.Models;

namespace ViewComponentExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("friends-List")]
        public IActionResult LoadFriendsList()
        {
            PersonGridModel model = new PersonGridModel
            {
                GridTitle = "Persons",
                People = new List<Person>
                {
                    new Person { PersonName = "John Doe", JobTitle = "Software Engineer" },
                    new Person { PersonName = "Jane Smith", JobTitle = "Project Manager" }
                }
            };
            return ViewComponent("Grid",new { grid = model});
        }
    }
}
