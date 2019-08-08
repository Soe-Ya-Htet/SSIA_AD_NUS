using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class RetrievalForm
    {
        public string binNo { get; set; }
        public long itemId { get; set; }

        public string description { get; set; }
        public int totalNeeded { get; set; }
        public int totalRetrieved { get; set; }
        public List<DeptNeeds> deptNeeds { get; set; }
    }

    public class DeptNeeds
    {
        public long deptId { get; set; }
        public string deptCode { get; set; }
        public int deptNeeded { get; set; }
        public int deptActual { get; set; }
        
    }
   
}