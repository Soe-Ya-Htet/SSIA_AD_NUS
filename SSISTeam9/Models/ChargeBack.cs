using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class ChargeBack
    {
        public long Id { get; set; }
        public int MonthOfOrder { get; set; }
        public int YearOfOrder { get; set; }
        public long DeptId { get; set; }
        public string DeptName { get; set; }
        public double Amount { get; set; }

    }
}