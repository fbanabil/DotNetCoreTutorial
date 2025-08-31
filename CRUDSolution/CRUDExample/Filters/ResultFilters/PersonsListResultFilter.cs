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

            await next();

            _logger.LogInformation("{FilterName}.{MethodName}: After executing the resource", nameof(PersonsListResultFilter), nameof(OnResourceExecutionAsync));

            context.HttpContext.Response.Headers["Last-Modified"] = DateTime.UtcNow.ToString("R");

        }
    }
}
