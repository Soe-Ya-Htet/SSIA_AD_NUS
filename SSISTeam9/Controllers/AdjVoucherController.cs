using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SSISTeam9.DAO;
using SSISTeam9.Models;

namespace SSISTeam9.Controllers
{
    public class AdjVoucherController : Controller
    {
        [HttpPost]
        public ActionResult Index(List<Inventory> inventories)
        {
            if (inventories == null)
            {
                return RedirectToRoute(new {controller = "Stock", action = "Check"});
            }
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();

            foreach (Inventory inventory in inventories)
            {
                int qty = inventory.ActualStock - inventory.StockLevel;
                if (qty != 0)
                {
                    PriceList list = PriceListDAO.GetPriceListById(inventory.ItemId) ?? new PriceList();
                    inventory.ItemSuppliersDetails = list;
                    AdjVoucher adjVoucher = new AdjVoucher
                    {
                        AdjQty = qty,
                        Item = inventory,
                        TotalPrice = inventory.ItemSuppliersDetails.Supplier1UnitPrice * qty
                    };
                    adjVouchers.Add(adjVoucher);
                }
            }

            return View(adjVouchers);
        }

        public ActionResult Generate(List<AdjVoucher> adjVouchers)
        {
            int managerId = 14;
            int supervisorId = 12;
            if (adjVouchers != null && adjVouchers.Count > 0)
            {
                for (var i = 0; i < adjVouchers.Count; i++)
                {
                    int id = (adjVouchers[i].TotalPrice < 250.0) ? supervisorId : managerId;
                    adjVouchers[i].AuthorisedBy = id.ToString();
                }
                AdjVoucherDAO.GenerateDisbursement(adjVouchers);
                AdjVoucherDAO.UpdateStock(adjVouchers);
            }

            ViewBag.Msg = "Success";
            return View(new List<AdjVoucher>());
        }

    }

}