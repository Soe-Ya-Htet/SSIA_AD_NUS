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

                string q = @"SELECT * from StockCard WHERE itemId = '" + ItemId +"'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StockCard stockCard = new StockCard()
                    {
                        CardId = (long)reader["cardId"],
                        ItemId = (long)reader["itemId"],
                        Date = (DateTime)reader["date"],
                        SourceType = (int)reader["sourceType"],
                        SourceId = (long)reader["sourceId"],
                        Qty = (string)reader["qty"],
                        Balance = (string)reader["balance"]
                    };
                    stockCards.Add(stockCard);
                }
            }
            return stockCards;
        }


    }
}