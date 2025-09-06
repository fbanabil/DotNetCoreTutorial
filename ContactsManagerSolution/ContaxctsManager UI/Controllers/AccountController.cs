using ContactsManager.Core.DTO;
using Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ContaxctsManager_UI.Controllers
{
    [Route("[Controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO mregisterDTO)
        {
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }
    }
}
