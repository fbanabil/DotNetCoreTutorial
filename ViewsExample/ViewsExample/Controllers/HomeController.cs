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
                    DateOfBirth = null,
                    PersonGender = Gender.Male
                }
            };



            //ViewData["title"]= title;  

            //ViewBag.title = title;
            ViewData["people"] = people;
            //ViewBag.people = people;
            return View("Index",people); // Views/Home/Index.cshtml   // @model using argument
            // o
            return new ViewResult { ViewName = "Index" };

        }

        [Route("person-details/{name}")]
        public IActionResult Details(string? name)
        {

            if(name==null)
            {
                return Content("Person name can't be null");
            }

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
                    DateOfBirth = null,
                    PersonGender = Gender.Male
                }
            };

            Person? matchingPerson = people.Where(temp=> temp.Name?.ToLower() == name?.ToLower()).FirstOrDefault();

            return View(matchingPerson);
        }


        [Route("person-with-product")]
        public IActionResult PersonWithProduct()
        {
            Person person = new Person
            {
                Name = "Fba Nabil",
                DateOfBirth = Convert.ToDateTime("2002-10-04"),
                PersonGender = Gender.Male
            };

            Product product = new Product
            {
                ProductId = 1,
                ProductName = "Laptop"
            };

            PersonAndProductWrapperModel model = new PersonAndProductWrapperModel
            {
                PersonData = person,
                ProductData = product
            };

            return View(model);
        }


        [Route("home/all-products")]
        public IActionResult All()
        {
            //Views/Products.All.cshtml
            // Views/Shared/All.cshtml
            return View();
        }
    }
}
