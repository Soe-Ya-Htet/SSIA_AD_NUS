using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class RequisitionDetails
    {
        public long ReqId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public Requisition Requisition { get; set; }
        public Inventory Item { get; set; }

    }
}