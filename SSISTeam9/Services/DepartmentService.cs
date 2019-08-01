﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class DepartmentService
    {
        public static bool VerifyExist(string deptCode)
        {
            List<string> deptCodes = DepartmentDAO.GetAllDepartmentCodes();
            foreach (string code in deptCodes)
            {
                if (deptCode == code)
                {
                    return true;
                }
            }
            return false;
        }

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
            DepartmentDAO.CreateDepartment(department);
        }

        public static void UpdateDepartment(Department department)
        {
            DepartmentDAO.UpdateDepartment(department);
        }

    }
}