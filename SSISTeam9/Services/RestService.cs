using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;

namespace SSISTeam9.Services
{
    public class RestService : IRestService
    {
        public List<Employee> GetAllEmployeesOfDepartment(long deptId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<RequisitionDetails>> GetAllPendingOrderDetailsByIdOfDepartment(int orderId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Requisition>> GetAllPendingOrdersOfDepartment(long deptId)
        {
            throw new NotImplementedException();
        }
    }
}