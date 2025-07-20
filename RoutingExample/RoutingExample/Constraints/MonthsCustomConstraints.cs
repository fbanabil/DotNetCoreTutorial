
using System.Text.RegularExpressions;

namespace RoutingExample.Constraints
{
    public class MonthsCustomConstraints : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if(!values.ContainsKey(routeKey))
            {
                return false;
            }
            
            Regex regex = new Regex("^(apr|jul|oct|jan)$", RegexOptions.IgnoreCase);
            string? montValue = values[routeKey]?.ToString();   

            if(regex.IsMatch(montValue ?? string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
