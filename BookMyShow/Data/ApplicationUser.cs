using BookMyShow.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BookMyShow.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public ICollection<Booking> Booking { get; set; }
    }
}
