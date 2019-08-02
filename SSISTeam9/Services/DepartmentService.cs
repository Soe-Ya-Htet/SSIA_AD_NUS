using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class DepartmentService
    {
        //public static bool VerifyExist(string deptCode)
        //{
        //    string input = deptCode.ToUpper();
        //    List<string> deptCodes = DepartmentDAO.GetAllDepartmentCodes();
        //    foreach (string code in deptCodes)
        //    {
        //        if (input == code)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static List<Department> GetAllDepartment()
        {
            return DepartmentDAO.GetAllDepartment();
        }

        public static Department GetDepartmentById(long itemId)
        {
            return DepartmentDAO.GetDepartmentById(itemId);
        }

        public static void DeleteDepartment(long itemId)
        {
            DepartmentDAO.DeleteDepartment(itemId);
        }

        public static void CreateDepartment(Department department)
        {
            string deptCode = department.DeptCode.ToUpper();           
            department.DeptCode = deptCode;
            DepartmentDAO.CreateDepartment(department);
        }

        public static void UpdateDepartment(Department department)
        {
            string deptCode = department.DeptCode.ToUpper();
            department.DeptCode = deptCode;
            DepartmentDAO.UpdateDepartment(department);
        }

    }
}