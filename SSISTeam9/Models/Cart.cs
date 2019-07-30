using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Cart
    {
        public long EmpId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
    }
}