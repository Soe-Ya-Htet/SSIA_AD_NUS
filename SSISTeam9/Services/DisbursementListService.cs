using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class DisbursementListService
    {

        public static List<DisbursementList> ViewOutstandingDisbursementsByCollection(string collectionPt)
        {
            List<DisbursementList> disbursementLists = DisbursementListDAO.ViewDisbursements(collectionPt);
            return disbursementLists;
        }

        public static List<DisbursementListDetails> ViewDisbursementDetails(long listId)
        {
            List<DisbursementListDetails> disbursementListDetails = DisbursementListDetailsDAO.ViewDetails(listId);
            return disbursementListDetails;
        }
        public static void CreateDisbursementLists(List<DisbursementList> disbursementLists)
        {
            foreach (var disbursementList in disbursementLists)
            {
                disbursementList.Department = DepartmentDAO.GetDepartmentById(disbursementList.Department.DeptId);
                long listId = DisbursementListDAO.CreateDisbursementList(disbursementList);
                foreach(var item in disbursementList.DisbursementListDetails)
                {
                    DisbursementListDetailsDAO.CreateDisbursementListDetails(listId, item);
                }
            }

            
        }

        public static void UpdateDisbursementListDetails(long listId, DisbursementListDetails disbursementListDetails)
        {

            DisbursementListDetailsDAO.UpdateDetailById(listId,disbursementListDetails);
        }

        public static List<DisbursementList> CheckForPendingDisbursements()
        {
            return DisbursementListDAO.CheckForPendingDisbursements();
        }

        //The following code is for ChargeBack controller
        public static DisbursementList GetDisbursementListByListId(long listId)
        {
            return DisbursementListDAO.GetDisbursementListByListId(listId);
        }

        public static DisbursementList GetDisbursementListByDeptId(long deptId)
        {
            return DisbursementListDAO.GetGeneratedDisbursementListByDeptId(deptId);
        }

        public static List<CollectionPoint> GetAllCollectionPoints()
        {
            return CollectionPointDAO.GetAllCollectionPoints();
        }

        public static void ChangeCollectionPoint(DisbursementList disbursement)
        {
            DisbursementListDAO.UpdateCollectionPoint(disbursement);
        }
    }
}