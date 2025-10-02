using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContaxctsManager_UI.Controllers
{
    [Route("[Controller]/[action]")]
    //[AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthenticated")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }


            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                PersonName = registerDTO.PersonName
            };

            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);

            if (result.Succeeded)
            {
                // Sign in the user
                if(registerDTO.UserTypes==ContactsManager.Core.Enums.UserTypesOptions.Admin)
                {
                    if(_roleManager.FindByNameAsync(UserTypesOptions.Admin.ToString()).Result==null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypesOptions.Admin.ToString() });
                    }

                    await _userManager.AddToRoleAsync(applicationUser, UserTypesOptions.Admin.ToString());

                }
                else
                {
                    if (_roleManager.FindByNameAsync(UserTypesOptions.User.ToString()).Result == null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypesOptions.User.ToString() });
                    }

                    await _userManager.AddToRoleAsync(applicationUser, UserTypesOptions.User.ToString());

                }

                await _signInManager.SignInAsync(applicationUser, isPersistent: false);

                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Resister", error.Description);
                }

                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }
        }





        [HttpGet]
        [Authorize("NotAuthenticated")]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthenticated")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (applicationUser != null)
                {
                    //bool isAdmin = await _userManager.IsInRoleAsync(applicationUser, UserTypesOptions.Admin.ToString());
                    if (await _userManager.IsInRoleAsync(applicationUser,UserTypesOptions.Admin.ToString()))
                    {
                        var userRoles = await _userManager.GetRolesAsync(applicationUser);
                        _logger.LogInformation("User {Email} logged in with roles: {Roles}", loginDTO.Email, string.Join(", ", userRoles));
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }

            ModelState.AddModelError("Login", "Invalid Login Attempt");
            ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
            return View(loginDTO);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(email);

            if (applicationUser == null)
            {
                return Json(true);
            }
            else return Json(false);
        }
    }
}
