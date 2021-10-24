using WhereIsMyShow.Data;
using WhereIsMyShow.Models;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UserRoleController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public  IActionResult NotFoundResult()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Index(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFoundResult");
            }

            var model = new List<UserRoleModel>();
            foreach (var user in userManager.Users)
            {
                var userRole = new UserRoleModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await userManager.IsInRoleAsync(user, role.Name)
                };


                model.Add(userRole);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<UserRoleModel> model, string roleId)
        {
            //Sanitized output from user
            var sanitizer = new HtmlSanitizer();
            roleId = sanitizer.Sanitize(roleId);
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFoundResult");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var userId = sanitizer.Sanitize(model[i].UserId);
                var user = await userManager.FindByIdAsync(userId);

                IdentityResult result;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Index", new { roleId });
                }
            }

            return RedirectToAction("Index", new { roleId });
        }


       
    }
}
