using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IActionResultExample.Models; 

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        public readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // /book?bookid=1&IsLoggedIn=true
        [Route("book")]
        public IActionResult Index(int? bookid, bool? IsLoggedIn)
        {
            // Use of model binding
            if(bookid.HasValue==false)
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("No book ID provided.");
                
                // Shortcuts
                return BadRequest("No book ID Provided");
            }

       

            // Check if the query string contains a specific key
            if (!Request.Query.ContainsKey("bookid")) // only bookid 
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("No book ID provided.");
                
                // Shortcuts
                return BadRequest("No book ID Provided");
            }
            // Book can not nbe empty
            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("Book ID cannot be empty.");
                
                // Shortcuts
                return BadRequest("Book ID cannot be empty.");

            }

            // Book ID should be betwwen 1 to 1000
            int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
            //_logger.LogInformation($"Book ID: {bookId}");
            if (bookId < 1 || bookId > 1000) 
            {
                //Response.StatusCode = 404; // Not Found
                //return Content("Book ID must be between 1 and 1000.");
                
                // Shortcuts
                return NotFound("Book ID must be between 1 and 1000.");
            }

            // Checking IsLoggedIn

            if (Convert.ToBoolean(Request.Query["IsLoggedIn"]) == false)
            {
                //Response.StatusCode = 401; // Unauthorized
                //return Content("You must be logged in to access this resource.");
                
                // Shortcuts
                return Unauthorized("You must be logged in to access this resource.");
                //return StatusCode(401);
            }

            return File("/sample.pdf", "application/pdf");
        }


        [Route("bookstore")]
        public IActionResult BookStore()
        {

            if (!Request.Query.ContainsKey("bookid"))
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("No book ID provided.");

                // Shortcuts
                return BadRequest("No book ID Provided");
            }
            // Book can not nbe empty
            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("Book ID cannot be empty.");

                // Shortcuts
                return BadRequest("Book ID cannot be empty.");

            }

            // Book ID should be betwwen 1 to 1000
            int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
           // _logger.LogInformation($"Book ID: {bookId}");
            if (bookId < 1 || bookId > 1000)
            {
                //Response.StatusCode = 404; // Not Found
                //return Content("Book ID must be between 1 and 1000.");

                // Shortcuts
                return NotFound("Book ID must be between 1 and 1000.");
            }

            // Checking IsLoggedIn

            if (Convert.ToBoolean(Request.Query["IsLoggedIn"]) == false)
            {
                //Response.StatusCode = 401; // Unauthorized
                //return Content("You must be logged in to access this resource.");

                // Shortcuts
                return Unauthorized("You must be logged in to access this resource.");
                //return StatusCode(401);
            }

            //return new RedirectToActionResult("Books", "Store", new {});//302 - Found

            // Shortcut
            return RedirectToAction("Books","Store", new { id = bookId}); //302 - Found : Search engine will not store the new url


            // Or

            //return new RedirectToActionResult("Books", "Store", new { }, true); //301 - moved permenently : Search engine will store the new url

            // shortcut

            //return RedirectToActionPermanent("Books", "Store", new { id = bookId }); //301 - moved permenently : Search engine will store the new url


            // Local Redirect 302
            //return new LocalRedirectResult("/store/books?id=" + bookId);
            //or
            //return LocalRedirect("/store/books?id=" + bookId); //302 - Found : Search engine will not store the new url

            // Local Redirect 301
            //return LocalRedirectPermanent("/store/books?id=" + bookId); //301 - moved permenently : Search engine will store the new url


            // Global Redirect 302
            //return new RedirectResult("https://www.google.com", true); //302 - Found : Search engine will not store the new url
                                                                                          // or
                                                                                          //return Redirect("https://www.google.com/search?q=bookstore", true); //302 - Found : Search engine will not store the new url

            // Global Redirect 301
            //return new RedirectResult("https://www.google.com", false); //301 - moved permenently : Search engine will store the new url
            //return RedirectPermanent("https://www.google.com"); //301 - moved permenently : Search engine will store the new url
        }

        //Route Parameter over Query Parameter
        // /bookCheck/1/true?bookid=5&IsLoggedIn=false
        // Will take bookid as 1 and IsLoggedIn as true
        [Route("bookCheck/{bookid?}/{IsLoggedIn?}")]
        public IActionResult BookCheck([FromRoute] int? bookid, [FromQuery] bool? IsLoggedIn) // Can use fromquery and from route
        {
            // Use of model binding
            if (bookid.HasValue == false)
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("No book ID provided.");

                // Shortcuts
                return BadRequest("No book ID Provided");
            }

            // Book ID should be betwwen 1 to 1000
            //_logger.LogInformation($"Book ID: {bookId}");
            if (bookid < 1 || bookid > 1000)
            {
                //Response.StatusCode = 404; // Not Found
                //return Content("Book ID must be between 1 and 1000.");

                // Shortcuts
                return NotFound("Book ID must be between 1 and 1000.");
            }

            // Checking IsLoggedIn

            if (IsLoggedIn.HasValue == false || IsLoggedIn.Value == false)
            {
                //Response.StatusCode = 401; // Unauthorized
                //return Content("You must be logged in to access this resource.");

                // Shortcuts
                return Unauthorized("You must be logged in to access this resource.");
                //return StatusCode(401);
            }

            return File("/sample.pdf", "application/pdf");
        }


        [Route("BookModel/{bookid?}/{IsLoggedIn?}")]
        public IActionResult BookModel([FromRoute] int? bookid, [FromQuery] bool? IsLoggedIn,
                Book book) // Can use fromquery and from route //model can be from qury string
        {
            // Use of model binding
            if (bookid.HasValue == false)
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("No book ID provided.");

                // Shortcuts
                return BadRequest("No book ID Provided");
            }

            // Book ID should be betwwen 1 to 1000
            //_logger.LogInformation($"Book ID: {bookId}");
            if (bookid < 1 || bookid > 1000)
            {
                //Response.StatusCode = 404; // Not Found
                //return Content("Book ID must be between 1 and 1000.");

                // Shortcuts
                return NotFound("Book ID must be between 1 and 1000.");
            }

            // Checking IsLoggedIn

            if (IsLoggedIn.HasValue == false || IsLoggedIn.Value == false)
            {
                //Response.StatusCode = 401; // Unauthorized
                //return Content("You must be logged in to access this resource.");

                // Shortcuts
                return Unauthorized("You must be logged in to access this resource.");
                //return StatusCode(401);
            }

            return Content($"Book Id :{bookid}, Book: {book}", "text/html");
        }


        // Form-urlencoded from postman
        // Formdata has higher priority than query string and route parameter
        [Route("BookForm/{bookid?}/{IsLoggedIn?}")]
        public IActionResult BookForm([FromRoute] int? bookid, [FromQuery]bool? IsLoggedIn,
                Book book) // Can use fromquery and from route //model can be from qury string
        {
            // Use of model binding
            if (bookid.HasValue == false)
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("No book ID provided.");

                // Shortcuts
                return BadRequest("No book ID Provided");
            }

            // Book ID should be betwwen 1 to 1000
            //_logger.LogInformation($"Book ID: {bookId}");
            if (bookid < 1 || bookid > 1000)
            {
                //Response.StatusCode = 404; // Not Found
                //return Content("Book ID must be between 1 and 1000.");

                // Shortcuts
                return NotFound("Book ID must be between 1 and 1000.");
            }

            // Checking IsLoggedIn

            if (IsLoggedIn.HasValue == false || IsLoggedIn.Value == false)
            {
                //Response.StatusCode = 401; // Unauthorized
                //return Content("You must be logged in to access this resource.");

                // Shortcuts
                return Unauthorized("You must be logged in to access this resource.");
                //return StatusCode(401);
            }

            return Content($"Book Id :{bookid}, Book: {book}", "text/html");
        }

        // Content-Type: multipart/form-data
        // For images or files
        [Route("BookFormData/{bookid?}/{IsLoggedIn?}")]
        public IActionResult BookFormData([FromRoute] int? bookid, [FromQuery] bool? IsLoggedIn,
                Book book) // Can use fromquery and from route //model can be from qury string
        {
            // Use of model binding
            if (bookid.HasValue == false)
            {
                //Response.StatusCode = 400; // Bad Request
                //return Content("No book ID provided.");

                // Shortcuts
                return BadRequest("No book ID Provided");
            }

            // Book ID should be betwwen 1 to 1000
            //_logger.LogInformation($"Book ID: {bookId}");
            if (bookid < 1 || bookid > 1000)
            {
                //Response.StatusCode = 404; // Not Found
                //return Content("Book ID must be between 1 and 1000.");

                // Shortcuts
                return NotFound("Book ID must be between 1 and 1000.");
            }

            // Checking IsLoggedIn

            if (IsLoggedIn.HasValue == false || IsLoggedIn.Value == false)
            {
                //Response.StatusCode = 401; // Unauthorized
                //return Content("You must be logged in to access this resource.");

                // Shortcuts
                return Unauthorized("You must be logged in to access this resource.");
                //return StatusCode(401);
            }

            return Content($"Book Id :{bookid}, Book: {book}", "text/html");
        }

    }
}
