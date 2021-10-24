using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Models
{
    public class ShowBooking
    {

       public int  ShowId {get;set;}

      public string ShowName { get; set; }

       public DateTime StartDate { get; set; }

      public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Number of Tickets ")]
        [Range(1, 5, ErrorMessage = "Number of Tickets Must be between 1 to 5")]
        public int NumberOfTickets { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Not a valid phone number")]

        public string PhoneNumber { get; set; }
    }
}
