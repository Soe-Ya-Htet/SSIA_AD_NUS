using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class StockCard
    {
        public long itemId { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public string Qty { get; set; }
        public string Balance { get; set; }
    }
}