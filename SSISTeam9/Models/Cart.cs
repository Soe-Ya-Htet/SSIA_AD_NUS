using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Cart
    {
        public int Quantity { get; set; }
        public Employee Employee { get; set; }
        public Inventory Item { get; set; }
    }
}