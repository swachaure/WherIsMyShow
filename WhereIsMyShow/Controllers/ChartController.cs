using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WhereIsMyShow.Data;
using WhereIsMyShow.Models;

namespace WhereIsMyShow.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class ChartController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ChartController(ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            var booking = dbContext.Bookings.ToList();
            List<ChartViewModel> dateChart = new List<ChartViewModel>();
            DateTime date = DateTime.Now;
            // Get days 
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var fithDayOfMonth = new DateTime(date.Year, date.Month, 5);
            var tenthDayOfMonth = new DateTime(date.Year, date.Month, 10);
            var fifteenDayOfMonth = new DateTime(date.Year, date.Month, 10);
            var twentyDayOfMonth = new DateTime(date.Year, date.Month, 20);
            var twentyfifthDay0fMonth = new DateTime(date.Year, date.Month, 25);
            var lasthDay0fMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //Count days
            ChartViewModel firstCount = new ChartViewModel()
            {
                Count = booking.Where(x => x.BookingDate >= firstDayOfMonth && x.BookingDate <= fithDayOfMonth).Count()
            };
            dateChart.Add(firstCount);

            ChartViewModel fifthCount = new ChartViewModel()
            {
                Count = booking.Where(x => x.BookingDate > fithDayOfMonth && x.BookingDate <= tenthDayOfMonth).Count()
            };
            dateChart.Add(fifthCount);

            ChartViewModel tenthCount = new ChartViewModel()
            {
                Count = booking.Where(x => x.BookingDate > tenthDayOfMonth && x.BookingDate <= fifteenDayOfMonth).Count()
            };

            dateChart.Add(tenthCount);


            ChartViewModel fifteenCount = new ChartViewModel()
            {
                Count = booking.Where(x => x.BookingDate > fifteenDayOfMonth && x.BookingDate <= twentyDayOfMonth).Count()
            };

            dateChart.Add(fifteenCount);

            ChartViewModel twentyCount = new ChartViewModel()
            {
                Count = booking.Where(x => x.BookingDate > twentyDayOfMonth && x.BookingDate <= twentyfifthDay0fMonth).Count()
            };

            dateChart.Add(twentyCount);

            ChartViewModel twentyfiveCount = new ChartViewModel()
            {
                Count = booking.Where(x => x.BookingDate > twentyfifthDay0fMonth && x.BookingDate <= lasthDay0fMonth).Count()
            };

            dateChart.Add(twentyfiveCount);

            List<int> list = dateChart.Select(x => x.Count).ToList();
            var data = JsonConvert.SerializeObject(list);
            ViewData["Data"] = HttpUtility.HtmlDecode(data);
            return View();
        }
    }
}
