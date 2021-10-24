using WhereIsMyShow.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage ="Booking Date is Required")]

        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Booking Status is Required")]
        public int Status { get; set; }

        [ForeignKey("Show")]
        public int ShowId { get; set; }

        public virtual Show Show { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser  ApplicationUser { get; set; }

        public int NumberOfTickets { get; set; }

        public string Name { get; set; }


        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }


    }
}
