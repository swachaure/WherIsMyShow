using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereIsMyShow.Data;
using WhereIsMyShow.Models;
using WhereIsMyShow.Utils;

namespace WhereIsMyShow.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly EmailSender sendGridEmailService;


        public BookingController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = applicationDbContext;
            this.userManager = userManager;
            this.sendGridEmailService = new EmailSender();

        }
        [HttpGet]
        public IActionResult Index(int id)
        {
            Show show = dbContext.Shows.FirstOrDefault(x => x.ShowId == id);
            if(show == null)
            {
                return RedirectToAction("ShowList","Show");
            }
            ShowBooking showBooking = new()
            {
                ShowId = show.ShowId,
                EndDate = show.StartDate.AddDays(15),
                StartDate = show.StartDate,
                ShowName = show.ShowName
            };
            return View(showBooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShowBooking showBooking)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUser();
                Booking booking = new()
                {
                    BookingDate = showBooking.StartDate,
                    ShowId = showBooking.ShowId,
                    Status = 0,
                    UserId = user.Id,
                    Name = showBooking.Name,
                    NumberOfTickets = showBooking.NumberOfTickets,
                    EmailAddress = showBooking.EmailAddress,
                    PhoneNumber = showBooking.PhoneNumber
                    
                };
                try
                {
                    dbContext.Bookings.Add(booking);
                    dbContext.SaveChanges();
                    SendEmailViewModel emailContract = new()
                    {
                        Contents = $"Hello {user.Name} You have succesfully Booked on {booking.BookingDate.ToShortDateString()} with Number of Tickets = {booking.NumberOfTickets} for Show {showBooking.ShowName}. Wait for Admin Approval",
                        Subject = "Pending Booking",
                        To = booking.EmailAddress
                    };
                    var response = sendGridEmailService.Send(emailContract);
                    return RedirectToAction("UserBooking");
                }
                catch
                {
                    return RedirectToAction("ShowList", "Show");
                }
                
            }
            return RedirectToAction("ShowList", "Show");

        }
      

            public async Task<IActionResult> UserBooking()
           {
            var user = await GetCurrentUser();

            var bookinglist = dbContext.Bookings.Where(x => x.UserId == user.Id).Include(x => x.Show).ToList();
            return View(bookinglist);
           }


        //Admin Portion

        [Authorize(Roles = "Admin,Staff")]
        public IActionResult AllBookings()
        {
            var bookinglist = dbContext.Bookings.Include(x => x.Show).Include(x => x.ApplicationUser).ToList();
            return View(bookinglist);
        }

        [Authorize(Roles = "Admin,Staff")]

        public IActionResult BulkBookingApprove()
        {

            List<Booking> bookings = dbContext.Bookings.Where(x => x.Status == 0).Include(x => x.Show).ToList();
            if (bookings.Count == 0)
            {
                return RedirectToAction("AllBookings");
            }
            List<BulkBookingUpdateViewModel> bulkBookingUpdateViewModels = bookings.Select(x => new BulkBookingUpdateViewModel { 
             ApplicationUser = x.ApplicationUser,
             BookingDate = x.BookingDate,
             isSelected = false,
             Status = 0,
             BookingId = x.BookingId,
             Show =x.Show,
             ShowId = x.ShowId,
             UserId = x.UserId
            
            }).ToList();
            return View(bulkBookingUpdateViewModels);
        }

        [Authorize(Roles = "Admin,Staff")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkBookingApprove(List<BulkBookingUpdateViewModel> bulkBookingUpdateViewModels)
        {
            try
            {
                List<int> idlist = bulkBookingUpdateViewModels.Where(x => x.isSelected = true).Select(x => x.BookingId).ToList();
                var bookinglist = dbContext.Bookings.Where(f => idlist.Contains(f.BookingId)).ToList();
                bookinglist.ForEach(a => a.Status = 1);
                dbContext.SaveChanges();
               
                foreach (Booking booking in bookinglist)
                {
                    var user = await userManager.FindByIdAsync(booking.UserId);
                    SendEmailViewModel emailContract = new()
                    {
                        Contents = $"Hello  We have succesfully Approved your booking. Details are as follows \n Name = {booking.Name}  \n Phone = {booking.PhoneNumber} \n Email Address = {booking.EmailAddress} \n Booking Date = {booking.BookingDate.ToShortDateString()} \n Number of Tickets =  {booking.NumberOfTickets}  ",
                        Subject = "Succesful Booking Done",
                        To = booking.EmailAddress
                    };
                    System.IO.File.WriteAllText(@"D:\uni\FIT5032-S2-2018-master\project\WhereIsMyShow\WhereIsMyShow\booking.txt", emailContract.Contents);
                    emailContract.Contents = "Please Find Attached";
                    var response = sendGridEmailService.SendEmail(emailContract);
                }
            }
            catch(Exception e)
            {
                
            }
            return RedirectToAction("BulkBookingApprove");
        }
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            var booking = dbContext.Bookings.Find(id);
            if (booking != null)
            {

                dbContext.Bookings.Remove(booking);
                dbContext.SaveChanges();
                return RedirectToAction("AllBookings");
            }
            ViewBag.Message = "Invalid Show Id";
            return RedirectToAction("Error", "Home");
        }


        private Task<ApplicationUser> GetCurrentUser() => userManager.GetUserAsync(HttpContext.User);
    }
}
