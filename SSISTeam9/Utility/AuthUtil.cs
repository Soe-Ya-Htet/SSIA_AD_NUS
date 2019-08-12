using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SSISTeam9.Utility
{
    public class AuthUtil
    {
        public static void CreatePrincipal(Employee emp)
        {
            Thread.CurrentPrincipal = new UserPrincipal(emp);
        }

        public static void CreatePrincipal(string username, string password, string role)
        {
            Thread.CurrentPrincipal = new UserPrincipal(username, password, role);
        }

        public static Employee GetCurrentLoggedUser()
        {
            //var principal = Thread.CurrentPrincipal;
            //return ((UserPrincipal)principal).Emp;

            UserPrincipal principal = (UserPrincipal) Thread.CurrentPrincipal;
            return principal.Emp;

        }

        public static void InvalidateUser()
        {
            Thread.CurrentPrincipal = null;
        }

    }
}