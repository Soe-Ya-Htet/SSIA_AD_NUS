using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Inventory
    {
        public long ItemId { get; }
        public string ItemCode { get; set; }
        public string BinNo { get; set; }
        public int StockLevel { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQty { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public string ImageUrl { get; set; }
    }
}