using BookMyShow.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Models
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


    }
}
