using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;


namespace CRUDExample.Filters.ActionFilters
{
    public class PersonCreatAndEditPostActionFilter : IAsyncActionFilter
    {

        private readonly ICountriesService _countriesService;
        
        public PersonCreatAndEditPostActionFilter(ICountriesService countryService)
        {
            _countriesService = countryService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse>? countries =
                    await _countriesService.GetAllCountries();

                    personsController.ViewBag.Countries = countries;

                    personsController.ViewBag.Countries = countries?.Select(temp =>
                    new SelectListItem()
                    {
                        Text = temp.CountryName,
                        Value = temp.CountryID.ToString()
                    });

                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    //ViewBag.PersonName = personAddRequest.PersonName;

                    var personRequest = context.ActionArguments["personRequest"];

                    context.Result = personsController.View(personRequest);//Short circuit the action execution
                }
                else
                {
                    await next();
                }

            }
            else
            {
                await next();
            }
            
        }
    }
}
