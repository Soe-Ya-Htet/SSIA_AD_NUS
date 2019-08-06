using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class DisbursementListDetailsDAO
    {
        public static List<DisbursementListDetails> ViewDetails(long listId)
        {
            List<DisbursementListDetails> disbursementListDetails = new List<DisbursementListDetails>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();


                string q = "SELECT * FROM DisbursementListDetails d, Inventory i WHERE d.itemId = i.itemId AND listId =" + listId;

                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory i = new Inventory()
                    {
                        ItemId = (long)reader["itemId"],
                        Description = (string)reader["description"]
                    };
                    DisbursementListDetails detail = new DisbursementListDetails
                    {
                        Quantity = (int)reader["quantity"],
                        Item = i
                        

                    };
                    disbursementListDetails.Add(detail);
                }
                
            }
            return disbursementListDetails;
        }
        
    }
}