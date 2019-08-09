using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class StockCardController : Controller
    {
        public ActionResult StockCard(long itemId)
        {
            Inventory catalogue = CatalogueService.GetCatalogueById(itemId);
            PriceList priceList = PriceListService.GetPriceListByItemId(itemId);
            List<StockCard> stockCards = StockCardService.GetStockCardById(itemId);

            ViewData["catalogue"] = catalogue;
            ViewData["priceList"] = priceList;
            ViewData["stockCards"] = stockCards;
            return View();
        }
    }
}