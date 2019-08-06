using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class PurchaseOrderDetails
    {
        public int Quantity { get; set; }
        public long OrderId { get; set; }
        public long ItemId { get; set; }
        public Inventory Item { get; set; }

        //Attributes for Analytics use case
        public int MonthOfOrder { get; set; }
        public int YearOfOrder { get; set; }
        public long SupplierId { get; set; }
        public double UnitPricePaid { get; set; }
        public string SupplierName { get; set; }
        public string ItemCategory { get; set; }
    }
}