using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("home")]
        public ContentResult Index()
        {
            // When no base class
            //return new ContentResult()
            //{
            //    Content="Hello from Home",
            //    ContentType = "text/plain",
            //};

            // Using Controller base class
            return Content("Hello from Home", "text/plain");
        }

        [Route("about")]
        public string About()
        {
            return "Hello from About";
        }

        [Route("contact-us/{mobile:regex(^\\d{{10}}$)}")]
        public string Contact()
        {
            return "Hello from Contact";
        }


        // Returning json

        [Route("person")]
        public JsonResult Person()
        {
            Person person= new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };

            //return new JsonResult(person);
            //or
            return Json(person);
        }

        // File downnload
        // If in webroot then virtualfileresult
        [Route("file-download")]
        public VirtualFileResult FileDownloaded()
        {
            //return new VirtualFileResult("/Fba_Nabil_CV.pdf", "application/pdf");
            //or
            return File("/Fba_Nabil_CV.pdf", "application/pdf");
        }

        // Outside webroot
        [Route("file-download2")]
        public PhysicalFileResult FileDownloaded2()
        {
            //return new PhysicalFileResult(@"C:\Users\ASUS\OneDrive\Desktop\Tour.jpg", "image/jpeg");
            //or 
            return PhysicalFile(@"C:\Users\ASUS\OneDrive\Desktop\Tour.jpg", "image/jpeg");
        }

        // Byte array from retreivation, like from database or another source
        [Route("file-download3")]
        public FileContentResult FileDownloaded3()
        {
            //return new PhysicalFileResult(@"C:\Users\ASUS\OneDrive\Desktop\Tour.jpg", "image/jpeg");
            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\ASUS\OneDrive\Desktop\Tour.jpg");

            //return new FileContentResult(bytes, "image/jpeg");
            //or
            return File(bytes, "image/jpeg");
        }
    }
}
