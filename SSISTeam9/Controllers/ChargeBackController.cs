using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class ChargeBackController : Controller
    {
        public ActionResult Select()
        {
            List<string> months = new List<string>
            {
                "January", "February", "March", "April", "May", "June", "July","August","September","October","November", "December"
            };
            List<int> years = new List<int>();
            int currentYear = DateTime.Now.Year;
            for(int i= 0; i < 5; i++)
            {
                years.Add(currentYear - i);
            }
            List<string> depts = DepartmentService.GetAllDepartmentNames();

            ViewData["months"] = months;
            ViewData["years"] = years;
            ViewData["depts"] = depts;

            List<ChargeBack> chargeBacks = (List<ChargeBack>)TempData["chargeBacks"];
            ViewData["chargeBacks"] = chargeBacks;
            int? mode = (int?)TempData["mode"];
            ViewData["mode"] = mode;
            ChargeBack chargeback = (ChargeBack)TempData["chargeback"];
            ViewData["chargeback"] = chargeback;
            return View();
        }

        public ActionResult Generate(int mode, ChargeBack chargeBack)
        {

            if (mode == 1)
            {
                List<ChargeBack> chargeBacks = ChargeBackService.GetChargeBackByMonth(chargeBack.MonthOfOrder);
                TempData["chargeBacks"] = chargeBacks;
                TempData["mode"] = mode;
                TempData["chargeback"] = chargeBack;
            }
            if(mode == 2)
            {
                chargeBack.DeptId = DepartmentService.GetDeptIdByName(chargeBack.DeptName);
                List<ChargeBack> chargeBacks = ChargeBackService.GetChargeBackByDept(chargeBack.DeptId, chargeBack.YearOfOrder);
                TempData["chargeBacks"] = chargeBacks;
                TempData["mode"] = mode;
                TempData["chargeback"] = chargeBack;
            }
        
            return RedirectToAction("Select");
        }


       

    }
}