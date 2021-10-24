using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Models
{
    public class SendEmailViewModel
    {


        public string From { get; set; }

        [Required(ErrorMessage = "Receiver Email Address is required")]
        [EmailAddress]
        public string To { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Contents are required")]

        
        [DataType(DataType.MultilineText)]
        public string Contents { get; set; }
    }
}
