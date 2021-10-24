using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Models
{
    public class AddShowModel : Show
    {
        [Required(ErrorMessage = "Please choose Show image")]
        [Display(Name = "Show Image")]
        public IFormFile Image { get; set; }
    }
}
