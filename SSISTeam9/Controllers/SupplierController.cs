using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }

        //Display all suppliers
        public ActionResult All()
        {
            return View();
        }

        public ActionResult CreateNew()
        {
            return View();
        }

        public ActionResult UpdateDetails()
        {
            return View();
        }

    }
}