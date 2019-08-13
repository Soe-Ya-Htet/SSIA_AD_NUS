using SSISTeam9.DAO;
using SSISTeam9.Filters;
using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SSISTeam9.Controllers.DisbursementController;

namespace SSISTeam9.Controllers
{
    [BasicAuthenticationAttribute]
    [RoutePrefix("rest/stock_clerk")]
    public class RestStockClerkController : Controller
    {
        private readonly IRestService restService;

        public RestStockClerkController(IRestService restService)
        {
            this.restService = restService;
        }

        [Route("retrievals")]
        public ActionResult RetrievalForms()
        {
            return Json(restService.GetAllRetrievalFormsOfStockClerk(), JsonRequestBehavior.AllowGet);
        }

        [Route("disbursement/generate")]
        public ActionResult GenerateDisbursement(List<Entry> entries)
        {
            return Json(restService.GenerateDisbursementOfStockClerk(entries), JsonRequestBehavior.AllowGet);
        }

        [Route("inventory/all")]
        public ActionResult GetAllInventories()
        {
            Dictionary<string, List<Inventory>> invDict = new Dictionary<string, List<Inventory>>
            {
                {"inventories", GetInventories() }
            };
            return Json(invDict, JsonRequestBehavior.AllowGet);
        }

        [Route("inventory/price_list")]
        public ActionResult GetInventoryPriceList(List<long> itemIds)
        {
            List<PriceList> priceLists = GetPriceListsByItemIds(itemIds);
            if(priceLists.Count != itemIds.Count)
            {
                if(priceLists.Count == 0)
                {
                    foreach(long itemId in itemIds)
                    {
                        PriceList priceList = new PriceList
                        {
                            Item = new Inventory
                            {
                                ItemId = itemId
                            },
                        };
                        priceLists.Add(priceList);
                    }
                }
                else
                {
                    foreach(long itemId in itemIds)
                    {
                        bool found = false;
                        foreach(PriceList priceList in priceLists)
                        {
                            if(itemId == priceList.Item.ItemId)
                            {
                                priceLists.Add(priceList);
                                found = true;
                                break;
                            }
                        }
                        if(!found)
                        {
                            PriceList priceList = new PriceList
                            {
                                Item = new Inventory
                                {
                                    ItemId = itemId
                                },
                            };
                            priceLists.Add(priceList);
                        }
                    }
                }
            }
            Dictionary<string, List<PriceList>> priceDict = new Dictionary<string, List<PriceList>>
            {
                {"priceLists",  priceLists}
            };
            return Json(priceDict, JsonRequestBehavior.AllowGet);
        }

        [Route("inventory/adjustment")]
        public ActionResult Generate(List<AdjVoucher> adjVouchers)
        {
            int managerId = 14;
            int supervisorId = 12;
            for (var i = 0; i < adjVouchers.Count; i++)
            {
                int id = (adjVouchers[i].TotalPrice < 250.0) ? supervisorId : managerId;
                adjVouchers[i].AuthorisedBy = id.ToString();
            }

            AdjVoucherDAO.GenerateDisbursement(adjVouchers);
            AdjVoucherDAO.UpdateStock(adjVouchers);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public List<Inventory> GetInventories()
        {
            return CatalogueService.GetAllCatalogue();
        }

        public static List<PriceList> GetPriceListsByItemIds(List<long> itemIds)
        {

            List<PriceList> pricelists = new List<PriceList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from PriceList WHERE itemId IN ({0})";

                var parms = itemIds.Select((s, i) => "@id" + i.ToString()).ToArray();
                var inclause = string.Join(",", parms);

                string sql = string.Format(q, inclause);

                SqlCommand cmd = new SqlCommand(sql, conn);
                for (var i = 0; i < parms.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parms[i], itemIds[i]);
                }

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    PriceList pricelist = new PriceList()
                    {
                        Item = new Inventory
                        {
                            ItemId = (long)reader["itemId"]
                        },
                        Supplier1Id = (long)reader["supplier1Id"],
                        Supplier2Id = (long)reader["supplier2Id"],
                        Supplier3Id = (long)reader["supplier3Id"],
                        Supplier1UnitPrice = (double)reader["supplier1UnitPrice"],
                        Supplier2UnitPrice = (double)reader["supplier2UnitPrice"],
                        Supplier3UnitPrice = (double)reader["supplier3UnitPrice"]
                    };

                    pricelists.Add(pricelist);
                }
            }
            return pricelists;

        }

    }
}