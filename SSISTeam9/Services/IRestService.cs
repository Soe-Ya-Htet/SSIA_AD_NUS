using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SSISTeam9.Controllers.DisbursementController;
using Delegate = SSISTeam9.Models.Delegate;

namespace SSISTeam9.Services
{
    public interface IRestService
    {
        Dictionary<string, object> Login(Employee emp);
        string Logout();
        Dictionary<string, List<Employee>> GetAllEmployeesOfDepartment();
        Dictionary<string, List<Requisition>> GetAllPendingOrdersOfDepartment();
        Dictionary<string, List<RequisitionDetails>> GetAllPendingOrderDetailsByIdOfDepartment(int orderId);
        Dictionary<string, object> GetAllRepresentativesOfDepartment();
        string ChangeRepresentativeOfDepartement(long repId);
        string ApproveOrdereOfDepartment(int reqId);
        string RejectOrdereOfDepartment(int reqId);
        Dictionary<string, List<Requisition>> GetAllPastOrdersOfDepartment();
        string DelegateAuthorityOfDepartment(Delegate delegat);
        Dictionary<string, object> GetDelegateInfoOfDepartment();
        string AcknowledgementOfRepresentative(long listId);
        Dictionary<string, List<DisbursementList>> GetAllPendingDisbursementsOfRep();
        Dictionary<string, List<DisbursementList>> GetAllPastDisbursementsOfRep();
        Dictionary<string, List<DisbursementListDetails>> GetAllDisbursementDetailsByIdOfRep(long listId);
        Dictionary<string, object> GetAllRetrievalFormsOfStockClerk();
        string GenerateDisbursementOfStockClerk(List<Entry> entries);

        Dictionary<string, List<Inventory>> GetAllInventories();
        Dictionary<string, List<PriceList>> GetAllInventoryPriceListByIds(List<long> itemIds);
        string SubmitStockAdjustment(List<AdjVoucher> adjVouchers);
        Dictionary<string, List<DisbursementList>> GetAllOutstandingDisbursementsOfClerk(long collectionPoint);
        string UpdateDisbursementsOfClerk(DisburmentDTO dto);
        Dictionary<string, List<CollectionPoint>> GetAllCollectionPoints();
    }
}
