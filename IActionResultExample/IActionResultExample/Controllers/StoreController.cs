using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class StoreController : Controller
    {
        [Route("store/books")]
        public IActionResult Books(int? id)
        {
            //int id= Convert.ToInt32(Request.Query["id"]);
            return Content($"<h1>Books List</h1><p>Here are some books available in our store. Book Id {id}</p>","text/html");
        }
    }
}
