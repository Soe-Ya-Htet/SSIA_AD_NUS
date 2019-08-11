using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SSISTeam9.Utility
{
    public class UserPrincipal : GenericPrincipal
    {
        public UserPrincipal(Employee emp) : this(emp.UserName, emp.Password, emp.EmpRole)
        {
            Emp = emp;
        }

        public UserPrincipal(string username, string password, string role) : base(new GenericIdentity(username, password), new string[] { role })
        {
            Emp = new Employee
            {
                UserName = username,
                Password = password,
                EmpRole = role
            };
        }

        public Employee Emp { get; }
    }
}