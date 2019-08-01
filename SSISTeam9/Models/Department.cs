using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Department
    {
        public long DeptId { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string Contact { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Head { get; set; }
        public Employee Representative { get; set; }
        public CollectionPoint CollectionPoint { get; set; }
    }
}