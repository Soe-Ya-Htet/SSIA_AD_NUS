using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class EmployeeService
    {
        public static Employee GetUserPassword(string userName)
        {
            return EmployeeDAO.GetUserPassword(userName);
        }

        public static Employee GetUserBySessionId(string sessionId)
        {
            return EmployeeDAO.GetUserBySessionId(sessionId);
        }

        public static Employee GetEmployeeById(long empId)
        {
            return EmployeeDAO.GetEmployeeById(empId);
        }

        public static string CreateSession(string userName)
        {
            return EmployeeDAO.CreateSession(userName);
        }

        public static bool IsActiveSessionId(string sessionId)
        {
            return EmployeeDAO.IsActiveSessionId(sessionId);
        }

        public static void RemoveSession(string sessionId)
        {
            EmployeeDAO.RemoveSession(sessionId);
        }
    }
}