using SSISTeam9.DAO;
using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Services
{
    public class RepresentativeService
    {
        public static List<Employee> GetEmployeesByDepartment(long deptId)
        {
            List<Employee> employeesWithoutHead = new List<Employee>();
            List<Employee> employees=EmployeeDAO.GetEmployeesByDepartment(deptId);
            foreach (Employee e in employees)
            {
                if (!e.EmpRole.Equals("HEAD"))
                {
                    employeesWithoutHead.Add(e);
                }
            }
            return employeesWithoutHead;
        }
        public static void UpdateEmployeeRole(long newRep,long currentRep,long deptId)
        {
            EmployeeDAO.UpdateEmployeeRoleById(newRep,currentRep);
            DepartmentDAO.UpdateDepartmentRepById(deptId,newRep);
        }
    }
}