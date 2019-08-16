using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class StockCardDAO
    {
        public static List<StockCard> GetStockCardById(long ItemId)
        {
            List<StockCard> stockCards = new List<StockCard>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                //display 50 rows of record
                string q = @"SELECT TOP 50 * FROM StockCard​ WHERE itemId = '" + ItemId + "'  ORDER BY date";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StockCard stockCard = new StockCard()
                    {
                        ItemId = (long)reader[1],
                        Date = (DateTime)reader[2],
                        SourceType = (int)reader[3],
                        SourceId = (long)reader[4],
                        Qty = (string)reader[5],
                        Balance = (int)reader[6]
                    };
                    stockCards.Add(stockCard);
                }
            }
            return stockCards;
        }

        public static void CreateStockCardFromDisburse(DisbursementListDetails disbursementDetails, DisbursementList disbursementList, int balance)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO StockCard (itemId,date,sourceType,sourceId,qty,balance)" +
                            "VALUES ('" + disbursementDetails.Item.ItemId +
                            "','" + disbursementList.date.Year +
                            "-" + disbursementList.date.Month +
                            "-" + disbursementList.date.Day +
                            "','2','" + disbursementList.Department.DeptId +
                            "','- " + disbursementDetails.Quantity +
                            "','" + balance + "')";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void CreateStockCardFromOrder(StockCard stockCard)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO StockCard (itemId,date,sourceType,sourceId,qty,balance)" +
                            "VALUES ('" + stockCard.ItemId +
                            "','" + stockCard.Date.Year +
                            "-" + stockCard.Date.Month +
                            "-" + stockCard.Date.Day +
                            "','3','" + stockCard.SourceId +
                            "','" + stockCard.Qty +
                            "','" + stockCard.Balance + "')";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}