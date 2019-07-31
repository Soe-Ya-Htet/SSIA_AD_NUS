using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class DisbursementList
    {
        public long ListId { get; set; }
        public long DeptId { get; set; }
        public long CollectionPointId { get; set; }
        public string AcknowledgeBy { get; set; }
        public DateTime date { get; set; }
        public Department Department { get; set; }
        public CollectionPoint CollectionPoint { get; set; }
    }
}