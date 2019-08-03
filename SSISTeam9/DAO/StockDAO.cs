using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class StockDAO
    {
        public static void UpdateInventoryStock(Dictionary<long, int> itemAndNewStock)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                foreach (KeyValuePair<long, int> item in itemAndNewStock)
                {
                    string q = @"UPDATE Inventory SET stockLevel = '" + item.Value + "' WHERE itemId = '" + item.Key + "'";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}