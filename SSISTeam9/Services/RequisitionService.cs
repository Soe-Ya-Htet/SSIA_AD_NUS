﻿using SSISTeam9.DAO;
using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SSISTeam9.Services
{
    public class RequisitionService
    {
        public static List<Inventory> GetAllInventory()
        {
            return CatalogueDAO.GetAllCatalogue();
        }
        public static List<Requisition> DisplayPendingRequisitions()
        {
            return RequisitionDAO.GetPendingRequisitionsFromDB();
        }

        public static List<Requisition> ShowAllOutstandingRequisitionsByDate()
        {
            List<Requisition> list = RequisitionDAO.GetPendingRequisitionsFromDB();

            if (list.Count == 0) return null;

            List<long> empIds = new List<long>();
            foreach (Requisition req in list)
            {
                empIds.Add(req.Employee.EmpId);
            }

            if (empIds.Count == 0) return null;

            List<Employee> employees = EmployeeDAO.GetEmployeesByIdList(empIds);
            if(employees.Count != 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i].Employee.EmpId == employees[i].EmpId)
                    {
                        list[i].Employee = employees[i];
                    }
                }
            }

            List<long> depIds = new List<long>();
            foreach (Requisition req in list)
            {
                depIds.Add(req.Employee.Department.DeptId);
            }

            List<Department> departments = new List<Department>();
            if (employees.Count != 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Employee.Department.DeptId == departments[i].DeptId)
                    {
                        list[i].Employee.Department = departments[i];
                    }
                }
            }

            return list;
        }
    }
}