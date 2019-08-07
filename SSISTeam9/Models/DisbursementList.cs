using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class DisbursementList
    {
        public long ListId { get; set; }
        public string AcknowledgedBy { get; set; }
        public DateTime date { get; set; }
        public Department Department { get; set; }
        public CollectionPoint CollectionPoint { get; set; }
        public List<DisbursementListDetails> DisbursementListDetails { get; set; }
    }
}