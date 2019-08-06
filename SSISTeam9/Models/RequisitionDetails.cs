using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class RequisitionDetails
    {
        public int Quantity { get; set; }
        public Requisition Requisition { get; set; }
        public Inventory Item { get; set; }

        //Attributes needed for analytics use case
        public long ItemId { get; set; }
        public string Category { get; set; }
        public int MonthOfRequest { get; set; }
        public int YearOfRequest { get; set; }
    }
}