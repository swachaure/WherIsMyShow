using BookMyShow.Data;
using BookMyShow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyShow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogInformation("Started Controlled");
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ShowList", "Show");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    PhoneNumber = model.Phone,
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    NormalizedEmail = model.Email,
                    NormalizedUserName = model.Name,

                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Home");
                }
                StringBuilder errorstring = new ();
                foreach (var error in result.Errors)
                {
                    errorstring.Append($"{error.Description}");
                    ModelState.AddModelError("", error.Description);
                }
                ViewBag.Error = errorstring.ToString();
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ShowList", "Show");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LogInViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("ShowList", "Show");
                }
                
                ViewBag.Error = "InValid Login";

            }
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
