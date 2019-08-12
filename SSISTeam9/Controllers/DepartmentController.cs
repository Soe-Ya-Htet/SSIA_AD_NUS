using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SSISTeam9.Models;
using SSISTeam9.DAO;
using SSISTeam9.Services;
using SSISTeam9.Filters;


namespace SSISTeam9.Controllers
{
    [StoreAuthorisationFilter]
    public class DepartmentController : Controller
    {

        public ActionResult All(string sessionId)
        {
            List<Department> departments = DepartmentService.GetAllDepartment();

            ViewData["departments"] = departments;
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult Delete(bool confirm, long deptId, string sessionId)
        {
            if (confirm)
            {
                DepartmentService.DeleteDepartment(deptId);

                List<Department> departments = DepartmentService.GetAllDepartment();
                ViewData["departments"] = departments;
                return RedirectToAction("All", new { sessionid = sessionId });
            }
            return null;
        }

        public ActionResult Create(string sessionId)
        {
            ViewData["empNames"] = EmployeeDAO.GetAllEmployeeNames();
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult CreateNew(Department department, string sessionId)
        {

            try
            {
                DepartmentService.CreateDepartment(department);

                List<Department> departments = DepartmentService.GetAllDepartment();
                ViewData["departments"] = departments;
            }
            catch (SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Department code already exists! Please Verify.');</script>";
                return RedirectToAction("Create");
            }
            return RedirectToAction("All", new { sessionid = sessionId });
        }

        public ActionResult Details(long deptId, string sessionId)
        {
            ViewData["department"] = DepartmentService.GetDepartmentById(deptId);
            ViewData["empNames"] = EmployeeDAO.GetAllEmployeeNames();
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult Update(Department department, string sessionId)
        {
            
            try
            {
                DepartmentService.UpdateDepartment(department);

                List<Department> departments = DepartmentService.GetAllDepartment();
                ViewData["departments"] = departments;
                return RedirectToAction("All", new { sessionid = sessionId });
            }
            catch (SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Department code already exists! Please Verify.');</script>";
                return RedirectToAction("Details", new { deptId = department.DeptId, sessionid = sessionId });
            }
        }
    }
}