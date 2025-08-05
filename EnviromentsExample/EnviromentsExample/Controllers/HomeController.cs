using Microsoft.AspNetCore.Mvc;

namespace EnviromentsExample.Controllers
{
    public class HomeController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }





        [Route("/")]
        [Route("some-route")]
        public IActionResult Index()
        {
            ViewBag.CurrentEnviroment = _webHostEnvironment.EnvironmentName;
            return View();
        }




        // Checking Error Page
        //[Route("some-route")]
        //public IActionResult Other()
        //{
        //    return View();
        //}
    }
}
