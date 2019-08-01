﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SSISTeam9.Models;
using SSISTeam9.DAO;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class DepartmentController : Controller
    {

        public ActionResult AllDepartment()
        {
            List<Department> departments = DepartmentService.GetAllDepartment();

            ViewData["departments"] = departments;
            return View();
        }

        public ActionResult Delete(bool confirm, long deptId)
        {
            if (confirm)
            {
                DepartmentService.DeleteDepartment(deptId);

                List<Department> departments = DepartmentService.GetAllDepartment();
                ViewData["departments"] = departments;
                return View("AllDepartment");
            }
            return null;
        }

        public ActionResult CreateDepartment()
        {
            return View();
        }

        public ActionResult CreateNew(Department department)
        {

            if (DepartmentService.VerifyExist(department.DeptCode))
            {
                TempData["errorMsg"] = "<script>alert('Department code already exists! Please Verify.');</script>";
                return View("CreateDepartment");
            }
            else
            {
                DepartmentService.CreateDepartment(department);

                List<Department> departments = DepartmentService.GetAllDepartment();
                ViewData["departments"] = departments;
            }
            return View("AllDepartment");
        }

        public ActionResult DisplayDepartmentDetails(long deptId)
        {
            ViewData["departments"] = DepartmentService.GetDepartmentById(deptId);
            return View();
        }

        public ActionResult UpdateDepartment(Department department)
        {
            DepartmentService.UpdateDepartment(department);

            List<Department> departments = DepartmentService.GetAllDepartment();
            ViewData["departments"] = departments;
            return View("AllDepartment");
        }
    }
}