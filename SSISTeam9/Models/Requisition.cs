using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Requisition
    {
        public long ReqId { get; set; }
        public long EmpId { get; set; }
        public string ReqCode { get; set; }
        public DateTime DateOfRequest { get; set; }
        public string Status { get; set; }
        public DateTime PickUpDate { get; set; }
        public string ApprovedBy { get; set; }
        public Employee Employee { get; set; }

    }
}