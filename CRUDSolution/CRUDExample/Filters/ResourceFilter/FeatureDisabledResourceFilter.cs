using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResourceFilter
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {

        private readonly ILogger<FeatureDisabledResourceFilter> _logger;
        private readonly bool isDisabled;

        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled = true)
        {
            _logger = logger;
            this.isDisabled = isDisabled;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // Before logic
            
            _logger.LogInformation("{FileName}.{MethodName} - after,",nameof(FeatureDisabledResourceFilter),nameof(OnResourceExecutionAsync));

            if (isDisabled)
            {
                context.Result=new NotFoundResult(); // 404 not found
            }
            else
            {
                await next();
            }

            // After logic

        }
    }
}
