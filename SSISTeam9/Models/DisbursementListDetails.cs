using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class DisbursementListDetails
    {
        public int Quantity { get; set; }
        public DisbursementList DisbursementList { get; set; }
        public Inventory Item { get; set; }
    }
}