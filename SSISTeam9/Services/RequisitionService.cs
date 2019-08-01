using SSISTeam9.DAO;
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
            List<Requisition> list = RequisitionDAO.GetRequisitionsByStatuses();
            List<long> empIds = new List<long>();
            foreach (Requisition req in list)
            {
                empIds.Add(req.Employee.EmpId);
                //req.Employee = EmployeeDAO.GetEmployeeById(req.Employee.EmpId);
            }

            List<Employee> employees = EmployeeDAO.GetEmployeesByIdList(empIds);
            if (employees.Count != 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Employee = employees.Find(e => e.EmpId == list[i].Employee.EmpId);

                }
            }
            return list;
        }

        public static List<RequisitionDetails> DisplayRequisitionDetailsByReqId(long reqId)
        {
            List<RequisitionDetails> list = RequisitionDAO.GetRequisitionDetailsByReqId(reqId);
            List<long> inventoryIds = new List<long>();
            foreach (RequisitionDetails req in list)
            {
                inventoryIds.Add(req.Item.ItemId);
                //req.Employee = EmployeeDAO.GetEmployeeById(req.Employee.EmpId);
            }

            List<Inventory> items = CatalogueDAO.GetInventoriesByIdList(inventoryIds);
            if (items.Count != 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Item.ItemId == items[i].ItemId)
                    {
                        list[i].Item = items[i];
                    }
                }
            }
            return list;
        }

        public static List<Requisition> ShowAllOutstandingRequisitionsByDate()
        {
            List<Requisition> list = RequisitionDAO.GetRequisitionsByStatuses("Approved","Partially Completed");

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
                    
                    list[i].Employee = employees.Find(e => e.EmpId == list[i].Employee.EmpId);
                    
                }
            }

            List<long> depIds = new List<long>();
            foreach (Requisition req in list)
            {
                depIds.Add(req.Employee.Department.DeptId);
            }

            List<Department> departments = DepartmentDAO.GetDepartmentsByIdList(depIds);
            if (employees.Count != 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    
                    list[i].Employee.Department = departments.Find(e => e.DeptId == list[i].Employee.Department.DeptId);
                   
                }
            }

            return list;
        }
    }
}