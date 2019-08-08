using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{

    [RoutePrefix("rest/dept_head")]
    public class RestDepartmentHeadController : Controller
    {
        private readonly int deptId = 1;

        private IEmailService emailService;

        public RestDepartmentHeadController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [Route("index")]
        public JsonResult Index()
        {
            Task.Run(() => emailService.SendEmail(""));

            return Json("Memory Loss", JsonRequestBehavior.AllowGet);
        }

        [Route("pending_orders")]
        public JsonResult GetAllPendingOrders()
        {
            List<Requisition> requisitions = RequisitionService.DisplayPendingRequisitions(deptId);
            Dictionary<string, List<Requisition>> reqOrderDict = new Dictionary<string, List<Requisition>> { };
            reqOrderDict.Add("reqList", requisitions);
            return Json(reqOrderDict, JsonRequestBehavior.AllowGet);
        }

        [Route("pending_order/{id:int}")]
        public JsonResult GetPendingOrderDetailsById(int id)
        {
            List<RequisitionDetails> requisitionOrderDetails = RequisitionService.DisplayRequisitionDetailsByReqId(id);
            Dictionary<string, List<RequisitionDetails>> reqOrderDict = new Dictionary<string, List<RequisitionDetails>> { };
            reqOrderDict.Add("reqDataList", requisitionOrderDetails);
            return Json(reqOrderDict, JsonRequestBehavior.AllowGet);
        }

        [Route("representatives")]
        public JsonResult GetAllRepresentatives()
        {

            long currentRepId = DepartmentService.GetCurrentRep(deptId);
            List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);
            Employee emp = employees.Find(e => e.EmpId == currentRepId);
            employees.Remove(emp);

            Dictionary<string, object> repDict = new Dictionary<string, object> { };
            repDict.Add("repList", employees);
            repDict.Add("curRep", emp);
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
        [Route("pending_order/approve")]
        public JsonResult ApproveOrder(int id)
        {
            return Json("Memory Loss", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("pending_order/reject")]
        public JsonResult RejectOrder()
        {
            return Json("Memory Loss", JsonRequestBehavior.AllowGet);
        }

        [Route("past_orders")]
        public JsonResult GetAllPastOrders()
        {
            //List<Requisition> reqs = RequisitionService.GetRequisitionByDeptId(deptId);
            List<Requisition> reqs = GetAllPastOrderReqs(deptId);
            Dictionary<string, List<Requisition>> reqDict = new Dictionary<string, List<Requisition>> { };
            reqDict.Add("reqList", reqs);
            return Json(reqDict, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("auth/delegate")]
        public JsonResult DelegateAuthority()
        {
            return Json("Memory Loss", JsonRequestBehavior.AllowGet);
        }

        private List<Requisition> GetAllPastOrderReqs(int deptId)
        {
            string sql = @"SELECT r.*, e.empName FROM Requisition r, Employee e WHERE r.empId=e.empId AND e.deptId=@deptId AND r.status != @status";
            return GetAllReqs(deptId, sql, "Pending Approval");
        }

        private List<Requisition> GetAllPendingOrderReqs(int deptId)
        {
            string sql = @"SELECT r.*, e.empName FROM Requisition r, Employee e WHERE r.empId=e.empId AND e.deptId=@deptId AND r.status = @status";
            return GetAllReqs(deptId, sql, "Pending Approval");

        }

        private List<Requisition> GetAllReqs(int deptId, string sql, string status)
        {
            List<Requisition> reqs = new List<Requisition>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                cmd.Parameters.AddWithValue("@status", status);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employee e = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        EmpName = (string) reader["empName"]
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