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
            return RequisitionDAO.GetPendingRequisitionsFromDB();
        }

        //public static List<Requisition> ShowAllOutstandingRequisitionsByDate()
        //{
        //    List<Requisition> list = RequisitionDAO.GetPendindingRequisition();
        //    foreach (Requisition req in list)
        //    {
        //        req.Employee = EmployeeDAO.GetEmployeeById(req.EmpId);
        //        req.Employee.Department = DepartmentDAO.GetDepartmentById(req.Employee.DeptId);

        //    }
        //    return list;
        //}
    }
}