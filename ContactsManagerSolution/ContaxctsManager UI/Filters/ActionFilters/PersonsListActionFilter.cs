using Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Filters.ActionFilters
{

    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter > _logger;


        public PersonsListActionFilter(ILogger<PersonsListActionFilter > logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // After Executed
            _logger.LogInformation("{FilterName}.{MethodName}: After executing the action method",nameof(PersonsListActionFilter),nameof(OnActionExecuted));

            PersonsController? personsController = context.Controller as PersonsController;

            IDictionary<string, object?>? actionArguments = context.HttpContext.Items["arguments"] as IDictionary<string, object?>;

            if(actionArguments!=null)
            {
                if(actionArguments.ContainsKey("searchBy"))
                {
                    personsController!.ViewData["SearchBy"] = actionArguments["searchBy"] as string;
                }
                if (actionArguments.ContainsKey("searchString"))
                {
                    personsController!.ViewData["SearchString"] = actionArguments["searchString"] as string;
                }
                if( actionArguments.ContainsKey("sortBy"))
                {
                    personsController!.ViewData["SortBy"] = Convert.ToString(actionArguments["sortBy"]);
                }
                else
                {
                    personsController!.ViewData["SortBy"] = nameof(PersonResponse.PersonName);
                }
                if (actionArguments.ContainsKey("sortOrder"))
                {
                    personsController!.ViewData["SortOrder"] = Convert.ToString(actionArguments["sortOrder"]);
                }
                else
                {
                    personsController!.ViewData["SortOrder"] = nameof(SortOrderOptions.ASC);
                }
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Before Executing
            _logger.LogInformation("{FilterName}.{MethodName}: After executing the action method", nameof(PersonsListActionFilter), nameof(OnActionExecuting));
            context.HttpContext.Items["arguments"] = context.ActionArguments;

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                

                string? searchBy = context.ActionArguments["searchBy"] as string;
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address)
                    };

                    if(searchOptions.Any(temp=>temp==searchBy))
                    {
                        _logger.LogInformation($"SearchBy parameter '{searchBy}' is valid.");
                    }
                    else
                    {
                        _logger.LogWarning($"SearchBy parameter '{searchBy}' is invalid.Setting it to {nameof(PersonResponse.PersonName)}");
                        //Reset parameter value
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    }
                }
            }

        }
    }
}
