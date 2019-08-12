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
        public static void DelegateToPreviousHead(long deptId)
        {
            long previousHead = DepartmentDAO.GetPreviousHead(deptId);
            long currentHead = DepartmentDAO.GetCurrentHeadById(deptId);
            DelegateDAO.DeleteDelegate(deptId,currentHead);
            EmployeeDAO.ChangeEmployeeRoles(deptId);
            DepartmentDAO.UpdateDepartmentHead(deptId, previousHead);
        }

        public static bool CheckDate(long deptId)
        {
            DateTime now = System.DateTime.Now;
            Models.Delegate d = DelegateDAO.GetDelegateByDept(deptId);
            if(now>=d.FromDate && now <= d.ToDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AfterDate(long deptId)
        {
            DateTime now = System.DateTime.Now;
            Models.Delegate d = DelegateDAO.GetDelegateByDept(deptId);
            if (now >= d.ToDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}