using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Models
{
    public class Show
    {
        public int ShowId { get; set; }

        [Required(ErrorMessage ="Show Name is required")]
        
        public string ShowName { get; set; }

        [Required(ErrorMessage = "Show Description is required")]

        public string Description { get; set; }

        [Required(ErrorMessage = "Genre of show is required")]
        public string Genre { get; set; }

        [Required(ErrorMessage ="Show Start Date is Required")]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage = "Show Start Time is Required")]
        public string StartTime { get; set; } 



        [Required(ErrorMessage = "Show End Time is Required")]
        public string EndTime { get; set; }

       public string ShowImage { get; set; }

        public ICollection<Booking> Booking { get; set; }

      


    }
}
