using SSISTeam9.DAO;
using SSISTeam9.Filters;
using SSISTeam9.Models;
using SSISTeam9.Utility;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    [RoutePrefix("rest")]
    public class RestLoginController : Controller
    {
        [BasicAuthenticationAttribute(aExecuting = false)]
        [Route("login")]
        [HttpPost]
        public ActionResult PostLogin(Employee emp)
        {
            Employee emp2 = EmployeeDAO.GetUserPassword(emp.UserName);
            if(emp2 == null || !emp.Password.Equals(emp2.Password))
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

            AuthUtil.CreatePrincipal(emp);
            return Json("Login", JsonRequestBehavior.AllowGet);
        }

        [BasicAuthenticationAttribute(rExecuted = false)]
        [Route("logout")]
        public ActionResult PostLogout()
        {
            AuthUtil.InvalidateUser();
            return Json("Logout", JsonRequestBehavior.AllowGet);
        }
    }
}