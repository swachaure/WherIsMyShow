using WhereIsMyShow.Data;
using WhereIsMyShow.Models;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WhereIsMyShow.Controllers
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
            //Sanitizing all input so no dangerous input can be send to controller
            var sanitizer = new HtmlSanitizer();
            model.Email = sanitizer.Sanitize(model.Email);
            model.ConfirmPassword = sanitizer.Sanitize(model.ConfirmPassword);
            model.Name = sanitizer.Sanitize(model.Name);
            model.Password = sanitizer.Sanitize(model.ConfirmPassword);
            model.Phone = sanitizer.Sanitize(model.Phone);
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
            // Sanitizing all input so no dangerous input can be send to controller
            var sanitizer = new HtmlSanitizer();
            user.Password = sanitizer.Sanitize(user.Password);
            user.Email = sanitizer.Sanitize(user.Email);
           
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

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Home");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
                return RedirectToAction("ShowList", "Show");
            else
            {
                ApplicationUser user = new()
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identResult = await userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent:true);
                        return RedirectToAction("ShowList", "Show");
                    }
                }
                return Error();
            }
        }
    }
}
