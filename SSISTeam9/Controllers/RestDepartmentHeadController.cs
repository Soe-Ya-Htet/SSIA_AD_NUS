﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{

    [RoutePrefix("Rest/DeptHead")]
    public class RestDepartmentHeadController : Controller
    {
        [Route("Index")]
        public JsonResult Index()
        {
            return Json("Memory Loss", JsonRequestBehavior.AllowGet);
        }

    }
}