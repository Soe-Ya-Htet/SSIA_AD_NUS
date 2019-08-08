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
        public long Amount { get; set; }

        public months Months { get; set; }
        public enum months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
    }
}