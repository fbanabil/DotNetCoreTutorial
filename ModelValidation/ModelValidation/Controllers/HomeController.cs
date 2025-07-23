using Microsoft.AspNetCore.Mvc;
using ModelValidation.CustomModelBinders;
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



        [Route("[action]")]
        public IActionResult BindTest([Bind(nameof(Person.PersonName),nameof(Person.Email)
            ,nameof(Person.Password),nameof(Person.ConfirmPassword))]Person person)//Only bind the specified properties
        {
            if (!ModelState.IsValid)
            {
                List<string> errorsList = ModelState.Values.SelectMany(value =>
                value.Errors).Select(err => err.ErrorMessage).ToList();
                string errors = string.Join("\n", errorsList);
                return BadRequest(errors);
            }
            return Content($"{person}");
        }


        [Route("[action]")]
        public IActionResult FromBodyTest([FromBody]Person person) // To bind the form body data like (json/xml)
        {
            if (!ModelState.IsValid)
            {
                List<string> errorsList = ModelState.Values.SelectMany(value =>
                value.Errors).Select(err => err.ErrorMessage).ToList();
                string errors = string.Join("\n", errorsList);
                return BadRequest(errors);
            }
            return Content($"{person}");
        }



        // This action method is used to test the custom model binder
        [Route("[action]")]
        public IActionResult CustomModelBinder([FromBody][ModelBinder(BinderType = typeof(CustomModelBinder))] Person person) 
        {
            if (!ModelState.IsValid)
            {
                List<string> errorsList = ModelState.Values.SelectMany(value =>
                value.Errors).Select(err => err.ErrorMessage).ToList();
                string errors = string.Join("\n", errorsList);
                return BadRequest(errors);
            }
            return Content($"{person}");
        }

        [Route("[action]")]
        public IActionResult CustomModelBinderProvider([FromBody] Person person) //Need to enable in program.cs
        {
            if (!ModelState.IsValid)
            {
                List<string> errorsList = ModelState.Values.SelectMany(value =>
                value.Errors).Select(err => err.ErrorMessage).ToList();
                string errors = string.Join("\n", errorsList);
                return BadRequest(errors);
            }
            return Content($"{person}");
        }


        //Header Binding
        [Route("[action]")]
        public IActionResult HeaderBinder([FromBody][ModelBinder(BinderType = typeof(CustomModelBinder))] Person person,
            [FromHeader(Name = "User-Agent")] string UserAgent)
        {
            if (!ModelState.IsValid)
            {
                List<string> errorsList = ModelState.Values.SelectMany(value =>
                value.Errors).Select(err => err.ErrorMessage).ToList();
                string errors = string.Join("\n", errorsList);
                return BadRequest(errors);
            }
            return Content($"{person}, {UserAgent}");
        }
    }
}
