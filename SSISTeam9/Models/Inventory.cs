using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSISTeam9.Models
{
    public class Inventory
    {
        public long ItemId { get; set; }

        [Display(Name = "Item Code")]
        [Required]
        public string ItemCode { get; set; }
        public string BinNo { get; set; }
        public int StockLevel { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQty { get; set; }
        public int ActualStock { get; set; }

        [Display(Name = "Category")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Unit Of Measure")]
        [Required]
        public string UnitOfMeasure { get; set; }
        public string ImageUrl { get; set; }
        public bool IsChecked { get; set; }
        public PriceList ItemSuppliersDetails { get; set; }

        public int PendingOrderQuantity;
        public List<bool> CheckedItems { get; set; }
        public List<Inventory> Items { get; set; }
        public int Flag { get; set; }
    }
}