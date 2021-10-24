using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Models
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }

        public string Id { get; set; }
    }
}
