using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class DisbursementListDetails
    {
        public long ListId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public DisbursementList DisbursementList { get; set; }
        public Inventory Item { get; set; }
    }
}