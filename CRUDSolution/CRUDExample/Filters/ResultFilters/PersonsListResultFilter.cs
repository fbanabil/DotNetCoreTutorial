using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncResourceFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;

        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName}: Before executing the resource", nameof(PersonsListResultFilter), nameof(OnResourceExecutionAsync));

            var executedContext = await next();

            _logger.LogInformation("{FilterName}.{MethodName}: After executing the resource", nameof(PersonsListResultFilter), nameof(OnResourceExecutionAsync));

            // Only set header if response hasn't started
            if (!executedContext.HttpContext.Response.HasStarted)
            {
                executedContext.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString();
            }

        }
    }
}
