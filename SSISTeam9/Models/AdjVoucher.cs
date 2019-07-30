using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class AdjVoucher
    {
        public long AdjId { get; set; }
        public long ItemId { get; set; }
        public DateTime Date { get; set; }
        public string AuthorisedBy { get; set; }
        public int AdjQty { get; set; }
        public string Reason { get; set; }
    }
}