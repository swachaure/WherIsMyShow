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
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {

          return  View(roleManager.Roles.Where(x => x.Name != "Admin"));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            //Sanitizing input
            var sanitizer = new HtmlSanitizer();
            model.RoleName = sanitizer.Sanitize(model.RoleName);
            model.Id = sanitizer.Sanitize(model.Id);
            if (ModelState.IsValid)
            {
                
                IdentityRole identityRole = new ()
                {
                    Name = model.RoleName
                };

               
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public IActionResult Edit(string role)
        {
            //Sanitizing input
            var sanitizer = new HtmlSanitizer();
            role = sanitizer.Sanitize(role);
            IdentityRole roleExists = roleManager.Roles.FirstOrDefault(x => x.Id == role);
            if(roleExists == null)
            {
               return RedirectToAction("Index");
            }
            RoleViewModel roleViewModel = new() { Id = roleExists.Id, RoleName = roleExists.Name };
            return View(roleViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            //Sanitizing input
            var sanitizer = new HtmlSanitizer();
            model.Id = sanitizer.Sanitize(model.Id);
            model.RoleName = sanitizer.Sanitize(model.RoleName);
            if (ModelState.IsValid)
            {

                IdentityRole roleExists = roleManager.Roles.FirstOrDefault(x => x.Id == model.Id);
                if (roleExists == null)
                {
                    return RedirectToAction("Index");
                }

                roleExists.Name = model.RoleName;
                roleExists.NormalizedName = model.RoleName;
                IdentityResult result = await roleManager.UpdateAsync(roleExists);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            //Sanitizing input
            var sanitizer = new HtmlSanitizer();
            id = sanitizer.Sanitize(id);
            IdentityRole roleExists = roleManager.Roles.FirstOrDefault(x => x.Id == id);
            if (roleExists != null)
            {

                var deleted = roleManager.DeleteAsync(roleExists);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
