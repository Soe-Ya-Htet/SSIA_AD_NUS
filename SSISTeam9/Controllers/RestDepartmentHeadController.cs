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
        //private readonly int deptId = 1;
        //private readonly int headId = 2;

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
            //Task.Run(() => emailService.SendEmail(""));

            //Dictionary<string, List<Employee>> repDict = new Dictionary<string, List<Employee>>();

            //Employee user = AuthUtil.GetCurrentLoggedUser();
            //if(user == null)
            //{
            //    repDict.Add("repList", new List<Employee>());
            //}
            //else
            //{
            //    List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(user.EmpId);
            //    repDict.Add("repList", employees);
            //}

            //return Json(repDict, JsonRequestBehavior.AllowGet);
            return Json(restService.GetAllEmployeesOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        [Route("pending_orders")]
        public JsonResult GetAllPendingOrders()
        {
            //Dictionary<string, List<Requisition>> reqOrderDict = new Dictionary<string, List<Requisition>>();

            //Employee user = AuthUtil.GetCurrentLoggedUser();
            //if (user == null)
            //{
            //    reqOrderDict.Add("reqList", new List<Requisition>());
            //}
            //else
            //{
            //    List<Requisition> requisitions = GetAllPendingOrderReqs((int)user.EmpId);
            //    reqOrderDict.Add("reqList", requisitions);
            //}

            //return Json(reqOrderDict, JsonRequestBehavior.AllowGet);
            return Json(restService.GetAllPendingOrdersOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        [Route("pending_order/{id:int}")]
        public JsonResult GetPendingOrderDetailsById(int id)
        {
            //List<RequisitionDetails> requisitionOrderDetails = RequisitionService.DisplayRequisitionDetailsByReqId(id);
            //Dictionary<string, List<RequisitionDetails>> reqOrderDict = new Dictionary<string, List<RequisitionDetails>>
            //{
            //    { "reqDataList", requisitionOrderDetails }
            //};
            //return Json(reqOrderDict, JsonRequestBehavior.AllowGet);
            return Json(restService.GetAllPendingOrderDetailsByIdOfDepartment(id), JsonRequestBehavior.AllowGet);
        }

        [Route("representatives")]
        public JsonResult GetAllRepresentatives()
        {

            //Dictionary<string, object> repDict = new Dictionary<string, object>();

            //Employee user = AuthUtil.GetCurrentLoggedUser();
            //if(user == null)
            //{
            //    repDict.Add("repList", new List<Employee>());
            //    repDict.Add("curRep", new Employee());
            //}
            //else
            //{
            //    long currentRepId = DepartmentService.GetCurrentRep(user.EmpId);
            //    List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(user.EmpId);
            //    Employee emp = employees.Find(e => e.EmpId == currentRepId);
            //    repDict.Add("repList", employees);
            //    repDict.Add("curRep", emp);
            //}

            //return Json(repDict, JsonRequestBehavior.AllowGet);
            return Json(restService.GetAllRepresentativesOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        [Route("representative/change/{id:long}")]
        public JsonResult ChangeRepresentative(long id)
        {
            //Employee user = AuthUtil.GetCurrentLoggedUser();
            //if(user == null)
            //{
            //    return Json("Failed", JsonRequestBehavior.AllowGet);
            //}

            //long currentRep = DepartmentService.GetCurrentRep(user.EmpId);
            //long newRep = id;
            //RepresentativeService.UpdateEmployeeRole(newRep, currentRep, user.EmpId);

            //return Json("Success", JsonRequestBehavior.AllowGet);
            return Json(restService.ChangeRepresentativeOfDepartement(id), JsonRequestBehavior.AllowGet);

        }

        [Route("pending_order/approve/{reqId:int}")]
        public JsonResult ApproveOrder(int reqId)
        {
            //Employee user = AuthUtil.GetCurrentLoggedUser();
            //if(user == null)
            //{
            //    return Json("Failed", JsonRequestBehavior.AllowGet);
            //}

            //long headId = DepartmentService.GetCurrentHead(user.EmpId);

            //RequisitionService.ProcessRequisition(reqId, "Approved", headId);
            //return Json("Success", JsonRequestBehavior.AllowGet);
            return Json(restService.ApproveOrdereOfDepartment(reqId), JsonRequestBehavior.AllowGet);
        }

        [Route("pending_order/reject/{reqId:int}")]
        public JsonResult RejectOrder(int reqId)
        {
            //Employee user = AuthUtil.GetCurrentLoggedUser();
            //if (user == null)
            //{
            //    return Json("Failed", JsonRequestBehavior.AllowGet);
            //}

            //long headId = DepartmentService.GetCurrentHead(user.EmpId);

            //RequisitionService.ProcessRequisition(reqId, "Rejected", headId);
            //return Json("Success", JsonRequestBehavior.AllowGet);
            return Json(restService.RejectOrdereOfDepartment(reqId), JsonRequestBehavior.AllowGet);
        }

        [Route("past_orders")]
        public JsonResult GetAllPastOrders()
        {
            //Dictionary<string, List<Requisition>> reqDict = new Dictionary<string, List<Requisition>>();

            //Employee user = AuthUtil.GetCurrentLoggedUser();
            //if(user == null)
            //{
            //    reqDict.Add("reqList", new List<Requisition>());
            //}
            //else
            //{
            //    List<Requisition> reqs = GetAllPastOrderReqs((int)user.EmpId);
            //    reqDict.Add("reqList", reqs);
            //}

            //return Json(reqDict, JsonRequestBehavior.AllowGet);
            return Json(restService.GetAllPastOrdersOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("auth/delegate")]
        public JsonResult DelegateAuthority(Delegate delegat)
        {
            //Employee user = AuthUtil.GetCurrentLoggedUser();
            //if (user == null)
            //{
            //    return Json("Failed", JsonRequestBehavior.AllowGet);
            //}

            //delegat.Department = new Department
            //{
            //    DeptId = user.EmpId
            //};

            //long headId = DepartmentService.GetCurrentHead(user.EmpId);

            //DelegateService.AddNewDelegate(delegat, headId);

            //return Json("Success", JsonRequestBehavior.AllowGet);
            return Json(restService.DelegateAuthorityOfDepartment(delegat), JsonRequestBehavior.AllowGet);
        }

        [Route("auth/delegate/info")]
        public JsonResult DelegateAuthorityInfo()
        {

            //Employee user = AuthUtil.GetCurrentLoggedUser();

            //if (user == null)
            //{
            //    return Json(null, JsonRequestBehavior.AllowGet);
            //}

            //Delegate del = GetDelegateInfoByDeptId((int)user.EmpId);

            //return Json(del, JsonRequestBehavior.AllowGet);
            return Json(restService.GetDelegateInfoOfDepartment(), JsonRequestBehavior.AllowGet);
        }

        //private Delegate GetDelegateInfoByDeptId(int deptId)
        //{
        //    Delegate del = null;

        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();
        //        string sql = @"SELECT * FROM Delegate WHERE deptId=@deptId";

        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@deptId", deptId);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            del = new Delegate
        //            {
        //                Employee = new Employee
        //                {
        //                    EmpId = (long)reader["empId"]
        //                },
        //                Department = new Department
        //                {
        //                    DeptId = (long)reader["deptId"]
        //                },
        //                FromDate = (DateTime)reader["fromDate"],
        //                ToDate = (DateTime)reader["toDate"]
        //            };
        //        }
        //    }
        //    return del;

        //}

        //private List<Requisition> GetAllPastOrderReqs(int deptId)
        //{
        //    string sql = @"SELECT r.*, e.empName FROM Requisition r, Employee e WHERE r.empId=e.empId AND e.deptId=@deptId AND r.status != @status";
        //    return GetAllReqs(deptId, sql);
        //}

        //private List<Requisition> GetAllPendingOrderReqs(int deptId)
        //{
        //    string sql = @"SELECT r.*, e.empName FROM Requisition r, Employee e WHERE r.empId=e.empId AND e.deptId=@deptId AND r.status = @status";
        //    return GetAllReqs(deptId, sql);

        //}

        //private List<Requisition> GetAllReqs(int deptId, string sql)
        //{
        //    List<Requisition> reqs = new List<Requisition>();

        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@deptId", deptId);
        //        cmd.Parameters.AddWithValue("@status", "Pending Approval");
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            Employee e = new Employee()
        //            {
        //                EmpId = (long)reader["empId"],
        //                EmpName = (string)reader["empName"]
        //            };

        //            Requisition requisition = new Requisition()
        //            {
        //                ReqId = (long)reader["reqId"],
        //                ReqCode = (string)reader["reqCode"],
        //                DateOfRequest = (DateTime)reader["dateOfRequest"],
        //                Status = (string)reader["status"],
        //                //PickUpDate = (DateTime)reader["pickUpDate"],
        //                ApprovedBy = (reader["approvedBy"] == DBNull.Value) ? "Nil" : (string)reader["approvedBy"],
        //                Employee = e
        //            };
        //            reqs.Add(requisition);
        //        }
        //    }

        //    return reqs;
        //}

    }
}