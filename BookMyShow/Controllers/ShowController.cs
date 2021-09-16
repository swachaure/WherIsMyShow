using BookMyShow.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Controllers
{
    [Authorize]
    public class ShowController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ShowController(ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }

        public IActionResult ShowList()
        {
            return View(dbContext.Shows.ToList());
        }
    }
}
