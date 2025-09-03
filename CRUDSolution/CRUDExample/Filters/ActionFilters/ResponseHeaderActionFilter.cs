using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{


    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        private readonly string _key;
        private readonly string _value;
        public int order;

        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order )
        {
            _key = key;
            _value = value;
            this.order = order;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            // Return Filter Object
           var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            filter._key = _key;
            filter._value = _value;
            filter.Order = order;
            
            return filter;
        }
    }



    public class ResponseHeaderActionFilter : IAsyncActionFilter,IOrderedFilter
    {
        public string _key { get; set; }
        public string _value{ get; set; }
        public int Order { get; set; }

        private readonly ILogger<ResponseHeaderActionFilter> _logger;

        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName}: Before executing the action method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            await next();
            _logger.LogInformation("{FilterName}.{MethodName}: After executing the action method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            context.HttpContext.Response.Headers[_key] = _value;
        }
    }
}
