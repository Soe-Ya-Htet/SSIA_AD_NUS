using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSISTeam9.Services
{
    interface IRestService
    {
        List<Employee> GetAllEmployeesOfDepartment(long deptId);
        Dictionary<string, List<Requisition>> GetAllPendingOrdersOfDepartment(long deptId);
        Dictionary<string, List<RequisitionDetails>> GetAllPendingOrderDetailsByIdOfDepartment(int orderId);
    }
}
