using Autofac;
using Microsoft.AspNetCore.Mvc;
//using Services;
using ServiceContracts;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICitiesService _citiesService1;
        private readonly ICitiesService _citiesService2;
        private readonly ICitiesService _citiesService3;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILifetimeScope _lifetimeScope;
        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3, IServiceScopeFactory serviceScopeFactory, ILifetimeScope lifetimeScope)
        {
            _citiesService1 = citiesService1;
            _citiesService3 = citiesService3;
            _citiesService2 = citiesService2;
            _serviceScopeFactory = serviceScopeFactory;
            _lifetimeScope = lifetimeScope;
        }


        // Method Injection Example
        //[Route("/")]
        //public IActionResult Index([FromServices] ICitiesService _citiesService)
        //{
        //    List<string> cities = _citiesService.GetCities();
        //    return View(cities);
        //}




        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities = _citiesService1.GetCities();
            ViewBag.InstanceId1 = _citiesService1.ServiceInstanceId;
            ViewBag.InstanceId2 = _citiesService2.ServiceInstanceId;
            ViewBag.InstanceId3 = _citiesService3.ServiceInstanceId;

            // ChildScope - can be used in a service

            using(IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                // Work like db
                ICitiesService _service = scope.ServiceProvider.GetRequiredService<ICitiesService>();

                // Different InstanceId
                ViewBag.InstanceId4 = _service.ServiceInstanceId;
            }

            using (ILifetimeScope scope = _lifetimeScope.BeginLifetimeScope())
            {
                // Work like db
                ICitiesService _service =scope.Resolve<ICitiesService>();

                // Different InstanceId
                ViewBag.InstanceId5 = _service.ServiceInstanceId;
            }


            return View(cities);
        }


    }
}
