using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.AuthorizationFilter
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext Context)
        {
            if(Context.HttpContext.Request.Cookies.ContainsKey("Auth-Key")==false)
            {
                Context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            if(Context.HttpContext.Request.Cookies["Auth-Key"]!="A100")
            {
                Context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
        }
    }
}
