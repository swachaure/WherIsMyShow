using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Models
{
    public class ShowViewModel
    {
        public int ShowId { get; set; }

        

        public string ShowName { get; set; }

       

        public string Description { get; set; }

       
        public string Genre { get; set; }

       
        public DateTime StartDate { get; set; }


      
        public string StartTime { get; set; }



        
        public string EndTime { get; set; }

        public string ShowImage { get; set; }

        public ICollection<Booking> Booking { get; set; }

        [Required]

        public int RatingStars { get; set; }

        public double TotalRating { get; set; }

    }
}
