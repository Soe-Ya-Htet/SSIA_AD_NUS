using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSISTeam9.Models
{
    public class AdjVoucher
    {
        public long AdjId { get; set; }
        public DateTime Date { get; set; }
        public long AuthorisedBy { get; set; }
        public int AdjQty { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@".\S+.", ErrorMessage = "No white space allowed")]
        public string Reason { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }

        public double TotalPrice { get; set; }

        //0-pending 1-approved 2-pending approve
        public int status { get; set; }
    }
}