using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Controllers;
using SSISTeam9.DAO;
using SSISTeam9.Models;
using SSISTeam9.Utility;

namespace SSISTeam9.Services
{
    public class RestService : IRestService
    {
        public string AcknowledgementOfRepresentative(long listId)
        {
            Employee emp = AuthUtil.GetCurrentLoggedUser();
            if (emp == null)
            {
                return "Failed";
            }

            DisbursementListDAO.AcknowledgeDisbursement(listId, emp.EmpId);

            return "Success";
        }

        public string ApproveOrdereOfDepartment(int reqId)
        {
            Employee user = AuthUtil.GetCurrentLoggedUser();
            if (user == null)
            {
                return "Failed";
            }

            long headId = DepartmentService.GetCurrentHead(user.EmpId);

            RequisitionService.ProcessRequisition(reqId, "Approved", headId);
            return "Success";
        }

        public string ChangeRepresentativeOfDepartement(long repId)
        {
            Employee user = AuthUtil.GetCurrentLoggedUser();
            if (user == null)
            {
                return "Failed";
            }

            long currentRep = DepartmentService.GetCurrentRep(user.EmpId);
            long newRep = repId;
            RepresentativeService.UpdateEmployeeRole(newRep, currentRep, user.EmpId);

            return "Success";
        }

        public string DelegateAuthorityOfDepartment(Models.Delegate delegat)
        {
            Employee user = AuthUtil.GetCurrentLoggedUser();
            if (user == null)
            {
                return "Failed";
            }

            delegat.Department = new Department
            {
                DeptId = user.EmpId
            };

            long headId = DepartmentService.GetCurrentHead(user.EmpId);

            DelegateService.AddNewDelegate(delegat, headId);

            return "Success";
        }

        public string GenerateDisbursementOfStockClerk(List<DisbursementController.Entry> entries)
        {
            if(entries == null || entries.Count == 0)
            {
                return "Failed";
            }

            HashSet<long> deptIds = new HashSet<long>();

            List<DisbursementList> disbursementLists = new List<DisbursementList>();

            entries.ForEach(e => deptIds.Add(e.deptId));

            foreach (var deptId in deptIds)
            {
                List<DisbursementListDetails> disbursementListDetails = new List<DisbursementListDetails>();

                foreach (var entry in entries)
                {
                    if (entry.deptId == deptId)
                    {
                        Inventory i = new Inventory()
                        {
                            ItemId = entry.itemId,
                        };
                        DisbursementListDetails dDets = new DisbursementListDetails()
                        {
                            Item = i,
                            Quantity = entry.quantity
                        };

                        disbursementListDetails.Add(dDets);
                    }
                }

                DisbursementList d = new DisbursementList()
                {
                    Department = new Department()
                    {
                        DeptId = deptId,
                    },
                    DisbursementListDetails = disbursementListDetails,
                    date = DateTime.Now
                };

                disbursementLists.Add(d);
            }

            DisbursementListService.CreateDisbursementLists(disbursementLists);

            return "Success";
        }

        public Dictionary<string, List<DisbursementListDetails>> GetAllDisbursementDetailsByIdOfRep(long listId)
        {
            List<DisbursementListDetails> disbursementDetails = DisbursementListDetailsDAO.ViewDetails(listId);

            Dictionary<string, List<DisbursementListDetails>> disDict = new Dictionary<string, List<DisbursementListDetails>>
            {
                { "disbursementDetails", disbursementDetails }
            };

            return disDict;
        }

        public Dictionary<string, List<Employee>> GetAllEmployeesOfDepartment()
        {
            Dictionary<string, List<Employee>> repDict = new Dictionary<string, List<Employee>>();

            Employee user = AuthUtil.GetCurrentLoggedUser();
            if (user == null)
            {
                repDict.Add("repList", new List<Employee>());
            }
            else
            {
                List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(user.EmpId);
                repDict.Add("repList", employees);
            }

            return repDict;
        }

        public Dictionary<string, List<DisbursementList>> GetAllPastDisbursementsOfRep()
        {
            Dictionary<string, List<DisbursementList>> disDict = new Dictionary<string, List<DisbursementList>>();

            Employee emp = AuthUtil.GetCurrentLoggedUser();
            if (emp == null)
            {
                disDict.Add("disbursementList", new List<DisbursementList>());
            }
            else
            {
                List<DisbursementList> disbursements = DisbursementListDAO.GetAllPastDisbursementList(emp.DeptId, emp.EmpId);
                disDict.Add("disbursementList", disbursements);
            }

            return disDict;
        }

        public Dictionary<string, List<Requisition>> GetAllPastOrdersOfDepartment()
        {
            Dictionary<string, List<Requisition>> reqDict = new Dictionary<string, List<Requisition>>();

            Employee user = AuthUtil.GetCurrentLoggedUser();
            if (user == null)
            {
                reqDict.Add("reqList", new List<Requisition>());
            }
            else
            {
                List<Requisition> reqs = RequisitionDAO.GetAllPastOrderReqs((int)user.EmpId);
                reqDict.Add("reqList", reqs);
            }

            return reqDict;
        }

        public Dictionary<string, List<DisbursementList>> GetAllPendingDisbursementsOfRep()
        {
            Dictionary<string, List<DisbursementList>> disDict = new Dictionary<string, List<DisbursementList>>();

            Employee emp = AuthUtil.GetCurrentLoggedUser();
            if (emp == null)
            {
                disDict.Add("disbursementList", new List<DisbursementList>());
            }
            else
            {
                List<DisbursementList> disbursements = DisbursementListDAO.GetAllPendingDisbursementList(emp.DeptId);
                disDict.Add("disbursementList", disbursements);
            }

            return disDict;
        }

        public Dictionary<string, List<RequisitionDetails>> GetAllPendingOrderDetailsByIdOfDepartment(int orderId)
        {
            List<RequisitionDetails> requisitionOrderDetails = RequisitionService.DisplayRequisitionDetailsByReqId(orderId);
            Dictionary<string, List<RequisitionDetails>> reqOrderDict = new Dictionary<string, List<RequisitionDetails>>
            {
                { "reqDataList", requisitionOrderDetails }
            };
            return reqOrderDict;
        }

        public Dictionary<string, List<Requisition>> GetAllPendingOrdersOfDepartment()
        {
            Dictionary<string, List<Requisition>> reqOrderDict = new Dictionary<string, List<Requisition>>();

            Employee user = AuthUtil.GetCurrentLoggedUser();
            if (user == null)
            {
                reqOrderDict.Add("reqList", new List<Requisition>());
            }
            else
            {
                List<Requisition> requisitions = RequisitionDAO.GetAllPendingOrderReqs((int)user.EmpId);

                reqOrderDict.Add("reqList", requisitions);
            }

            return reqOrderDict;
        }

        public Dictionary<string, object> GetAllRepresentativesOfDepartment()
        {
            Dictionary<string, object> repDict = new Dictionary<string, object>();

            Employee user = AuthUtil.GetCurrentLoggedUser();
            if (user == null)
            {
                repDict.Add("repList", new List<Employee>());
                repDict.Add("curRep", new Employee());
            }
            else
            {
                long currentRepId = DepartmentService.GetCurrentRep(user.EmpId);
                List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(user.EmpId);
                Employee emp = employees.Find(e => e.EmpId == currentRepId);
                repDict.Add("repList", employees);
                repDict.Add("curRep", emp);
            }

            return repDict;
        }

        public Dictionary<string, List<RetrievalForm>> GetAllRetrievalFormsOfStockClerk()
        {
            List<RetrievalForm> retrievalForms = RetrievalFormService.ViewRetrievalForm();

            Dictionary<string, List<RetrievalForm>> retrievalDict = new Dictionary<string, List<RetrievalForm>>
            {
                { "retrievalForms", retrievalForms }
            };

            return retrievalDict;
        }

        public Models.Delegate GetDelegateInfoOfDepartment()
        {
            Employee user = AuthUtil.GetCurrentLoggedUser();

            if (user == null)
            {
                return null;
            }

            Models.Delegate del = DelegateDAO.GetDelegateInfoByDeptId((int)user.EmpId);

            return del;
        }

        public Dictionary<string, object> Login(Employee emp)
        {
            Dictionary<string, object> resDict = new Dictionary<string, object>();

            Employee emp2 = EmployeeDAO.GetUserPassword(emp.UserName);
            if (emp2 == null || string.IsNullOrEmpty(emp2.UserName))
            {
                resDict.Add("login", false);
                resDict.Add("msg", "Username not found");
            }
            else if (!emp.Password.Equals(emp2.Password))
            {
                resDict.Add("login", false);
                resDict.Add("msg", "Incorrect password");
            }
            else
            {
                resDict.Add("login", true);
                resDict.Add("msg", "Login success");
                resDict.Add("emp", emp2);
                AuthUtil.CreatePrincipal(emp2);
            }

            return resDict;
        }

        public string Logout()
        {
            AuthUtil.InvalidateUser();
            return "Success";
        }

        public string RejectOrdereOfDepartment(int reqId)
        {
            Employee user = AuthUtil.GetCurrentLoggedUser();
            if (user == null)
            {
                return "Failed";
            }

            long headId = DepartmentService.GetCurrentHead(user.EmpId);

            RequisitionService.ProcessRequisition(reqId, "Rejected", headId);
            return "Success";
        }
    }
}