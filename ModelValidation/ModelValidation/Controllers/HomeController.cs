using Microsoft.AspNetCore.Mvc;
using ModelValidation.Models;

namespace ModelValidation.Controllers
{
    public class HomeController : Controller
    {
        [Route("register")]
        public IActionResult Index(Person person)
        {
            if(!ModelState.IsValid)
            {
                //List<string> errorsList = new List<string>();
                //foreach (var value in ModelState.Values)
                //{
                //    foreach(var error in value.Errors)
                //    {
                //        errorsList.Add(error.ErrorMessage);
                //    }
                //}
                //string errors = string.Join("\n", errorsList);

                // or
                
                //List<string> errorsList = ModelState.Values.SelectMany(values =>
                //values.Errors.Select(err => err.ErrorMessage)).ToList();
                //string errors = string.Join("\n", errorsList);
                
                //or
                List<string> errorsList = ModelState.Values.SelectMany(value =>
                value.Errors).Select(err => err.ErrorMessage).ToList();
                string errors = string.Join("\n", errorsList);
                return BadRequest(errors);

                // If the model state is invalid, return the validation errors
                //return BadRequest(ModelState);
            }
            return Content($"{person}");
        }
    }
}
