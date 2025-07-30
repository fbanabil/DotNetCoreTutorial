using Microsoft.AspNetCore.Mvc;
using PartialViewsExample.Models;

namespace PartialViewsExample.Controllers
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

        [Route("programming-languages")]
        public IActionResult ProgrammingLanguages()
        {
            ListModel listModel = new ListModel
            {
                Cities = new List<string>
                {
                    "C#",
                    "JavaScript",
                    "Python",
                    "Java",
                    "Ruby"
                },
                ListTitle = "Popular Programming Languages"
            };
            //return PartialView("_ListPartialView", listModel);
            return PartialView("_ListPartialView", listModel);
        }
    }
}
