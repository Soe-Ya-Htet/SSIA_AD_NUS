using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;
using SSISTeam9.Filters;

namespace SSISTeam9.Controllers
{
    public class StockCardController : Controller
    {
        [StoreAuthorisationFilter]
        public ActionResult StockCard(long itemId, string sessionId)
        {
               
            ViewData["catalogue"] = CatalogueService.GetCatalogueById(itemId);
            ViewData["priceList"] = PriceListService.GetPriceListByItemId(itemId);
            ViewData["stockCards"] = StockCardService.GetStockCardById(itemId);
            ViewData["sessionId"] = sessionId;
            return View();
        }
    }
}