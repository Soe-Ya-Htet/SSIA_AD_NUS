using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Delegate = SSISTeam9.Models.Delegate;

namespace SSISTeam9.Controllers
{

    [RoutePrefix("rest/dept_head")]
    public class RestDepartmentHeadController : Controller
    {
        private readonly int deptId = 1;
        private readonly int headId = 2;

        private IEmailService emailService;

        public RestDepartmentHeadController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [Route("employees")]
        public JsonResult Index()
        {
            //Task.Run(() => emailService.SendEmail(""));

            List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);

            Dictionary<string, List<Employee>> repDict = new Dictionary<string, List<Employee>>
            {
                { "repList", employees }
            };

            return Json(repDict, JsonRequestBehavior.AllowGet);
        }

        [Route("pending_orders")]
        public JsonResult GetAllPendingOrders()
        {
            //List<Requisition> requisitions = RequisitionService.DisplayPendingRequisitions(deptId);
            List<Requisition> requisitions = GetAllPendingOrderReqs(deptId);
            Dictionary<string, List<Requisition>> reqOrderDict = new Dictionary<string, List<Requisition>>
            {
                { "reqList", requisitions }
            };
            return Json(reqOrderDict, JsonRequestBehavior.AllowGet);
        }

        [Route("pending_order/{id:int}")]
        public JsonResult GetPendingOrderDetailsById(int id)
        {
            List<RequisitionDetails> requisitionOrderDetails = RequisitionService.DisplayRequisitionDetailsByReqId(id);
            Dictionary<string, List<RequisitionDetails>> reqOrderDict = new Dictionary<string, List<RequisitionDetails>>
            {
                { "reqDataList", requisitionOrderDetails }
            };
            return Json(reqOrderDict, JsonRequestBehavior.AllowGet);
        }

        [Route("representatives")]
        public JsonResult GetAllRepresentatives()
        {

            long currentRepId = DepartmentService.GetCurrentRep(deptId);
            List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);
            Employee emp = employees.Find(e => e.EmpId == currentRepId);
            employees.Remove(emp);

            Dictionary<string, object> repDict = new Dictionary<string, object>
            {
                { "repList", employees },
                { "curRep", emp }
            };
            return Json(repDict, JsonRequestBehavior.AllowGet);
        }

        [Route("representative/change/{id:long}")]
        public JsonResult ChangeRepresentative(long id)
        {
            long currentRep = DepartmentService.GetCurrentRep(deptId);
            long newRep = id;
            RepresentativeService.UpdateEmployeeRole(newRep, currentRep, deptId);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("pending_order/approve/{reqId:int}")]
        public JsonResult ApproveOrder(int reqId)
        {
            RequisitionService.ProcessRequisition(reqId, "Approved", headId);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("pending_order/reject/{reqId:int}")]
        public JsonResult RejectOrder(int reqId)
        {
            RequisitionService.ProcessRequisition(reqId, "Rejected", headId);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [Route("past_orders")]
        public JsonResult GetAllPastOrders()
        {
            //List<Requisition> reqs = RequisitionService.GetRequisitionByDeptId(deptId);
            List<Requisition> reqs = GetAllPastOrderReqs(deptId);
            Dictionary<string, List<Requisition>> reqDict = new Dictionary<string, List<Requisition>>
            {
                { "reqList", reqs }
            };
            return Json(reqDict, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("auth/delegate")]
        public JsonResult DelegateAuthority(Delegate delegat)
        {
            delegat.Department = new Department
            {
                DeptId = deptId
            };
            DelegateService.AddNewDelegate(delegat, headId);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        private List<Requisition> GetAllPastOrderReqs(int deptId)
        {
            string sql = @"SELECT r.*, e.empName FROM Requisition r, Employee e WHERE r.empId=e.empId AND e.deptId=@deptId AND r.status != @status";
            return GetAllReqs(deptId, sql);
        }

        private List<Requisition> GetAllPendingOrderReqs(int deptId)
        {
            string sql = @"SELECT r.*, e.empName FROM Requisition r, Employee e WHERE r.empId=e.empId AND e.deptId=@deptId AND r.status = @status";
            return GetAllReqs(deptId, sql);

        }

        private List<Requisition> GetAllReqs(int deptId, string sql)
        {
            List<Requisition> reqs = new List<Requisition>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                cmd.Parameters.AddWithValue("@status", "Pending Approval");
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employee e = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        EmpName = (string)reader["empName"]
                    };

                    Requisition requisition = new Requisition()
                    {
                        ReqId = (long)reader["reqId"],
                        ReqCode = (string)reader["reqCode"],
                        DateOfRequest = (DateTime)reader["dateOfRequest"],
                        Status = (string)reader["status"],
                        //PickUpDate = (DateTime)reader["pickUpDate"],
                        ApprovedBy = (reader["approvedBy"] == DBNull.Value) ? "Nil" : (string)reader["approvedBy"],
                        Employee = e
                    };
                    reqs.Add(requisition);
                }
            }

            return reqs;
        }

    }
}