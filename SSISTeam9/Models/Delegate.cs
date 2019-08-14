using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Delegate
    {
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }
        [Display(Name = "Employee Name")]
        public Employee Employee { get; set; }
        public Department Department { get; set; }
    }
}