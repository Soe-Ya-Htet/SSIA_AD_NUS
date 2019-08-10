using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string UserName, string Password)
        {
            if (UserName == null)
                return View();

            UserInfo user = UserData.GetUserPassword(UserName);

            //Check if MD5 hash of entered pwd matches that in system.
            using (MD5 md5Hash = MD5.Create())
            {
                string hashPwd = MD5Hash.GetMd5Hash(md5Hash, Password);

                if (user == null)
                    return View();

                if (user.Password != hashPwd)
                    return View();
            }

            string SessionId = ShoppingCart.Models.Session.CreateSession(UserName);

            return RedirectToAction("ViewProducts", "Gallery", new { username = UserName, sessionid = SessionId });
        }

        public ActionResult Logout(string sessionId)
        {
            ShoppingCart.Models.Session.RemoveSession(sessionId);
            return RedirectToAction("Login", "Home");
        }
    }
}