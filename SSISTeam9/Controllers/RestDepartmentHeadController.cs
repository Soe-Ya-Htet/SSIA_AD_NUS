using SSISTeam9.Filters;
using SSISTeam9.Models;
using SSISTeam9.Services;
using SSISTeam9.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Delegate = SSISTeam9.Models.Delegate;

namespace SSISTeam9.Controllers
{
    [BasicAuthenticationAttribute]
    [RoutePrefix("rest/dept_head")]
    public class RestDepartmentHeadController : Controller
    {
        private readonly IEmailService emailService;
        private readonly IRestService restService;

        public RestDepartmentHeadController(IEmailService emailService, IRestService restService)
        {
            this.emailService = emailService;
            this.restService = restService;
        }

        [Route("employees")]
        public JsonResult GetEmployees()
        {
            return Json(restService.GetAllEmployeesOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        [Route("pending_orders")]
        public JsonResult GetAllPendingOrders()
        {
            return Json(restService.GetAllPendingOrdersOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        [Route("pending_order/{id:int}")]
        public JsonResult GetPendingOrderDetailsById(int id)
        {
            return Json(restService.GetAllPendingOrderDetailsByIdOfDepartment(id), JsonRequestBehavior.AllowGet);
        }

        [Route("representatives")]
        public JsonResult GetAllRepresentatives()
        {
            return Json(restService.GetAllRepresentativesOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        [Route("representative/change/{id:long}")]
        public JsonResult ChangeRepresentative(long id)
        {
            return Json(restService.ChangeRepresentativeOfDepartement(id), JsonRequestBehavior.AllowGet);

        }

        [Route("pending_order/approve/{reqId:int}")]
        public JsonResult ApproveOrder(int reqId)
        {
            return Json(restService.ApproveOrdereOfDepartment(reqId), JsonRequestBehavior.AllowGet);
        }

        [Route("pending_order/reject/{reqId:int}")]
        public JsonResult RejectOrder(int reqId)
        {
            return Json(restService.RejectOrdereOfDepartment(reqId), JsonRequestBehavior.AllowGet);
        }

        [Route("past_orders")]
        public JsonResult GetAllPastOrders()
        {
            return Json(restService.GetAllPastOrdersOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("auth/delegate")]
        public JsonResult DelegateAuthority(Delegate delegat)
        {
            return Json(restService.DelegateAuthorityOfDepartment(delegat), JsonRequestBehavior.AllowGet);
        }

        [Route("auth/delegate/info")]
        public JsonResult DelegateAuthorityInfo()
        {
            return Json(restService.GetDelegateInfoOfDepartment(), JsonRequestBehavior.AllowGet);
        }

    }
}