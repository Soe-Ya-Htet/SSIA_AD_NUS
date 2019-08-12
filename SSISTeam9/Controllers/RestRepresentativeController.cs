using SSISTeam9.DAO;
using SSISTeam9.Filters;
using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    [BasicAuthenticationAttribute]
    [RoutePrefix("rest/representative")]
    public class RestRepresentativeController : Controller
    {
        private readonly int repId = 1;
        private readonly int deptId = 1;

        [Route("disbursement/acknowledge/{id:long}")]
        public ActionResult Acknowledge(long id)
        {
            AcknowledgeDisbursement(id, repId);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [Route("disbursements/pending")]
        public ActionResult GetAllPendingDisbursements()
        {
            List<DisbursementList> disbursements = GetAllPendingDisbursementList(deptId);

            Dictionary<string, List<DisbursementList>> disDict = new Dictionary<string, List<DisbursementList>>
            {
                { "disbursementList", disbursements }
            };

            return Json(disDict, JsonRequestBehavior.AllowGet);
        }

        [Route("disbursements/past")]
        public ActionResult GetAllPastDisbursements()
        {
            List<DisbursementList> disbursements = GetAllPastDisbursementList(deptId, repId);

            Dictionary<string, List<DisbursementList>> disDict = new Dictionary<string, List<DisbursementList>>
            {
                { "disbursementList", disbursements }
            };

            return Json(disDict, JsonRequestBehavior.AllowGet);
        }

        [Route("disbursement/{listId:long}")]
        public ActionResult GetAllPendingDisbursementDetailsList(long listId)
        {
            List<DisbursementListDetails> disbursementDetails = DisbursementListDetailsDAO.ViewDetails(listId);

            Dictionary<string, List<DisbursementListDetails>> disDict = new Dictionary<string, List<DisbursementListDetails>>
            {
                { "disbursementDetails", disbursementDetails }
            };

            return Json(disDict, JsonRequestBehavior.AllowGet);
        }

        private List<DisbursementList> GetAllPendingDisbursementList(long deptId)
        {
            List<DisbursementList> disbursements = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * FROM DisbursementList d, CollectionPoint cP, Department dept 
                            WHERE d.collectionPointId= cP.placeId AND acknowledgedBy IS NULL AND d.deptId = dept.deptId
                            AND d.deptId = @deptId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
 
                    DisbursementList disbursement = new DisbursementList
                    {
                        ListId = (long)reader["listId"],
                        date = (DateTime)reader["date"]
                    };

                    disbursement.Department = new Department()
                    {
                        DeptId = (long)reader["deptId"],
                        DeptName = (string) reader["deptName"]
                    };
                    disbursement.CollectionPoint = new CollectionPoint()
                    {
                        PlacedId = (long)reader["placeId"],
                        Name = (string)reader["name"]
                    };

                    disbursements.Add(disbursement);
                }
            }
            return disbursements;

        }

        private List<DisbursementList> GetAllPastDisbursementList(long deptId, long empId)
        {
            List<DisbursementList> disbursements = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * FROM DisbursementList d, CollectionPoint cP, Department dept 
                            WHERE d.collectionPointId= cP.placeId AND acknowledgedBy=@empId AND d.deptId = dept.deptId
                            AND d.deptId = @deptId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                cmd.Parameters.AddWithValue("@empId", empId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    DisbursementList disbursement = new DisbursementList
                    {
                        ListId = (long)reader["listId"],
                        date = (DateTime)reader["date"]
                    };

                    disbursement.Department = new Department()
                    {
                        DeptId = (long)reader["deptId"],
                        DeptName = (string)reader["deptName"]
                    };
                    disbursement.CollectionPoint = new CollectionPoint()
                    {
                        PlacedId = (long)reader["placeId"],
                        Name = (string)reader["name"]
                    };

                    disbursements.Add(disbursement);
                }
            }
            return disbursements;

        }


        private void AcknowledgeDisbursement(long listId, long empId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"Update DisbursementList Set acknowledgedBy = @empId where listId = @listId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@empId", empId);
                cmd.Parameters.AddWithValue("@listId", listId);
                cmd.ExecuteNonQuery();
            }
        }

    }
}