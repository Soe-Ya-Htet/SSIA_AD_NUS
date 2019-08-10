using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Employee
    {
        public long EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpRole { get; set; }
        public string EmpDisplayRole { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Department Department { get; set; }
        public string SessionId { get; set; }
    }
}