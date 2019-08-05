using SSISTeam9.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Services
{
    public class DelegateService
    {
        public static void AddNewDelegate(Models.Delegate d,long currentHead)
        {
            DelegateDAO.InsertNewDelegate(d);
            EmployeeDAO.UpdateEmployeeHead(d.Employee.EmpId, currentHead);
            DepartmentDAO.UpdateDepartmentHead(d.Department.DeptId,d.Employee.EmpId);
        }
    }
}