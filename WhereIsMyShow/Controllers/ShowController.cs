using WhereIsMyShow.Data;
using WhereIsMyShow.Models;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace WhereIsMyShow.Controllers
{
    [Authorize]
    public class ShowController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IWebHostEnvironment webHostEnvironment;

        public ShowController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager,IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = applicationDbContext;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> ShowList()
        {
            List<Show> shows = dbContext.Shows.ToList();
            List<ShowViewModel> models = new();

            foreach(var show in shows)
            {
                ShowViewModel model = new()
                {
                    Description = show.Description,
                    EndTime = show.EndTime,
                    Genre = show.Genre,
                    ShowImage = show.ShowImage,
                    StartTime = show.StartTime,
                    StartDate = show.StartDate,
                    ShowId = show.ShowId,
                    ShowName = show.ShowName,
                    RatingStars = 0,
                    TotalRating = 5

                };
                List<ShowRating> ratings = dbContext.ShowRatings.Where(x => x.ShowId == show.ShowId).ToList();
                if (ratings.Count != 0)
                {
                    var user = await GetCurrentUser();
                    ShowRating showRating = ratings.FirstOrDefault(x => x.UserId == user.Id);
                    if (showRating != null)
                    {
                        model.RatingStars = showRating.RatingStars;
                    }
                    var currentshowratings = ratings.Select(x => x.RatingStars).Average();
                    model.TotalRating = currentshowratings;
                }

                models.Add(model);
            }
            return View(models);
        }

        [Authorize(Roles ="Admin,Staff")]
        [HttpGet]
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddShowModel showModel)
        {
            var sanitizer = new HtmlSanitizer();
            showModel.StartTime = sanitizer.Sanitize(showModel.StartTime);
            showModel.EndTime = sanitizer.Sanitize(showModel.EndTime);
            showModel.Description = sanitizer.Sanitize(showModel.Description);
            showModel.Genre = sanitizer.Sanitize(showModel.Genre);
            showModel.ShowName = sanitizer.Sanitize(showModel.ShowName);
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(showModel);
                Show currentshow = new()
                {
                    ShowImage = uniqueFileName,
                    Description = showModel.Description,
                    EndTime = showModel.EndTime,
                    ShowName = showModel.ShowName,
                    Genre = showModel.Genre,
                    StartDate = showModel.StartDate,
                    StartTime = showModel.StartTime

                };
                dbContext.Shows.Add(currentshow);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(ShowList));
            }
            return View(showModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
           

            var show = await dbContext.Shows.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            AddShowModel showModel = new()
            {
                 ShowId = show.ShowId,
                 ShowImage = show.ShowImage,
                 Description = show.Description,
                 Genre = show.Genre,
                 EndTime = show.EndTime,
                 StartDate = show.StartDate,
                 StartTime = show.StartTime,
                 ShowName = show.ShowName
                  
            };
            return View(showModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( AddShowModel showModel)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(showModel);
                    Show show = dbContext.Shows.FirstOrDefault(x => x.ShowId == showModel.ShowId);
                    show.ShowImage = uniqueFileName;
                    show.ShowName = showModel.ShowName;
                    show.StartDate = showModel.StartDate;
                    show.StartTime = showModel.StartTime;
                    show.EndTime = showModel.EndTime;
                    show.Description = showModel.Description;
                    show.Genre = showModel.Genre;

                    dbContext.Shows.Update(show);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                        return NotFound();
                  
                }
                return RedirectToAction(nameof(ShowList));
            }
            return View(showModel);
        }




        [HttpGet]
        public async Task<IActionResult> ShowDetails(int id)
        {

            Show show = dbContext.Shows.FirstOrDefault(x => x.ShowId == id);
            if(show == null)
            {
                return RedirectToAction("ShowList");
            }
            ShowViewModel model = new()
            {
                 Description = show.Description,
                 EndTime = show.EndTime,
                 Genre   = show.Genre,
                 ShowImage = show.ShowImage,
                 StartTime = show.StartTime,
                 StartDate = show.StartDate,
                 ShowId = show.ShowId,
                 ShowName = show.ShowName,
                 RatingStars = 0,
                 TotalRating = 5

            };
            List<ShowRating> ratings = dbContext.ShowRatings.Where(x => x.ShowId == show.ShowId).ToList();
            if(ratings.Count != 0)
            {
                var user = await GetCurrentUser();
                ShowRating showRating = ratings.FirstOrDefault(x => x.UserId == user.Id);
                if(showRating != null)
                {
                    model.RatingStars = showRating.RatingStars;
                }
                var currentshowratings = ratings.Select(x => x.RatingStars).Average();
                model.TotalRating = currentshowratings;
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RatingsUpdate(ShowViewModel model)
        {
            var user = await GetCurrentUser();
            ShowRating alreadyRated = dbContext.ShowRatings.FirstOrDefault(x => x.ShowId == model.ShowId && x.UserId == user.Id);
            if (alreadyRated != null)
            
            {
                alreadyRated.RatingStars = model.RatingStars;
                dbContext.ShowRatings.Update(alreadyRated);
                await dbContext.SaveChangesAsync();

            }
            else
            {
                alreadyRated = new ShowRating
                {
                    RatingStars = model.RatingStars,
                    ShowId = model.ShowId,
                    UserId = user.Id
                };
                dbContext.ShowRatings.Add(alreadyRated);
                await dbContext.SaveChangesAsync();
            }


                return RedirectToAction("ShowList", new { id = model.ShowId });
        }


            [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            var show = dbContext.Shows.Find(id);
            if (show != null)
            {

                dbContext.Remove(show);
                dbContext.SaveChanges();
                return RedirectToAction("ShowList");
            }
            ViewBag.Message = "Invalid Show Id";
            return RedirectToAction("Error", "Home");
        }

        private Task<ApplicationUser> GetCurrentUser() => userManager.GetUserAsync(HttpContext.User);

        private string UploadedFile(AddShowModel model)
        {
            string uniqueFileName = !string.IsNullOrEmpty(model.ShowImage) ? model.ShowImage : null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath,"img");

                if (string.IsNullOrEmpty(model.ShowImage))
                {

                    uniqueFileName =Guid.NewGuid().ToString() + "_" + model.Image.FileName;

                }

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Image.CopyTo(fileStream);
            }
            return uniqueFileName;
        }


    }
}
