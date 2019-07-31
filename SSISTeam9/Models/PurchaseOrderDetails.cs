using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class PurchaseOrderDetails
    {
        public long OrderId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public PurchaseOrder Order { get; set; }
        public Inventory Item { get; set; }
    }
}