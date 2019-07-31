using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Delegate
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Employee Employee { get; set; }
        public Department Department { get; set; }
    }
}