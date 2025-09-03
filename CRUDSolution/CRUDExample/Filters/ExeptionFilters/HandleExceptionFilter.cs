using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace CRUDExample.Filters.ExeptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IHostEnvironment _service;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment service)
        {
            _logger = logger;
            _service = service;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "{FilterName}.{MethodName}: Exception occurred: {ExceptionMessage}", nameof(HandleExceptionFilter), nameof(OnException), context.Exception.Message);

            if (_service.IsDevelopment())
            {
                context.Result = new ContentResult()
                {
                    Content = context.Exception.Message,
                    ContentType = "text/plain",
                    StatusCode = 500
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
