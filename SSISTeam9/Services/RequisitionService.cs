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
            List<Inventory> itemList = new List<Inventory>();
            itemList.Add(CatalogueDAO.GetCatalogueById(1));
            return itemList;
            //return CatalogueDAO.DisplayAllCatalogue();
        }

        public static void SaveToCart(Cart cart)
        {
            List<Cart> carts = CartDAO.GetAllCart();
            carts = GetCartsWithObjects(carts);

            if (null != carts &&
                null != carts.Find(c => c.Employee.EmpId == cart.Employee.EmpId && c.Item.ItemId == cart.Item.ItemId))
            {
                CartDAO.UpdateCart(cart);
            }
            else
            {
                CartDAO.SaveCart(cart);
            }


        }

        private static List<Cart> GetCartsWithObjects(List<Cart> carts)
        {
            if (carts.Count == 0) return null;
            List<long> empIds = new List<long>();
            List<long> itemIds = new List<long>();
            foreach (Cart c in carts)
            {
                empIds.Add(c.Employee.EmpId);
                itemIds.Add(c.Item.ItemId);
            }

            if (empIds.Count == 0 || itemIds.Count == 0) return null;


            List<Employee> employees = EmployeeDAO.GetEmployeesByIdList(empIds);
            List<Inventory> inventories = CatalogueDAO.GetCataloguesByIdList(itemIds);
            if (employees.Count != 0)
            {
                for (int i = 0; i < carts.Count; i++)
                {
                    carts[i].Employee = employees.Find(e => e.EmpId == carts[i].Employee.EmpId);
                    carts[i].Item = inventories.Find(item => item.ItemId == carts[i].Item.ItemId);
                }
            }

            return carts;
        }

        public static List<Requisition> DisplayPendingRequisitions(long deptId)
        {
            string[] status = { "Pending Approval"};
            List<Requisition> list = RequisitionDAO.GetRequisitionsByStatuses(status);
            List<Requisition> filtered=new List<Requisition>();
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
                    if (list[i].Employee.Department.DeptId == deptId)
                    {
                        filtered.Add(list[i]);
                    }

                }
            }
            return filtered;
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

        public static void ProcessRequisition(long reqId, string status, long currentHead)
        {

            RequisitionDAO.UpdateRequisitionStatus(reqId, status, currentHead);
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