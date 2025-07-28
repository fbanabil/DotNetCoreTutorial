using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            string title = "Asp.Net Core App";

            List<Person> people = new List<Person>
            {
                new Person
                {
                    Name = "Fba Nabil",
                    DateOfBirth = Convert.ToDateTime("2002-10-04"),
                    PersonGender = Gender.Male
                },
                new Person
                {
                    Name = "Denesh Barua",
                    DateOfBirth = Convert.ToDateTime("2002-06-19"),
                    PersonGender = Gender.Other
                },
                new Person
                {
                    Name = "Atfan Bin Nur",
                    DateOfBirth = Convert.ToDateTime("2000-03-24"),
                    PersonGender = Gender.Male
                }
            };



            ViewData["title"]= title;
            ViewData["people"] = people;
            return View(); // Views/Home/Index.cshtml
            // o
            return new ViewResult { ViewName = "Index" };

        }
    }
}
