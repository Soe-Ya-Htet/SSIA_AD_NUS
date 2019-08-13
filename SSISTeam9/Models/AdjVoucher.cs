using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class AdjVoucher
    {
        public long AdjId { get; set; }
        public DateTime Date { get; set; }
        public long AuthorisedBy { get; set; }
        public int AdjQty { get; set; }
        public string Reason { get; set; }
        public long ItemId { get; set; }

        public double TotalPrice { get; set; }

        //0-pending 1-pending approve 2-approved
        public int status { get; set; }
    }
}