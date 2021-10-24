using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WhereIsMyShow.Data;

namespace WhereIsMyShow.Models
{
    public class ShowRating
    {
        public int ShowRatingId { get; set; }

       

        [Required]
        public int RatingStars { get; set; }

        [ForeignKey("Show")]
        public int ShowId { get; set; }

        public virtual Show Show { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
