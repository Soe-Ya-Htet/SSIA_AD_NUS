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
                        Description = (string)reader["description"],
                        UnitOfMeasure = (string)reader["unitOfMeasure"]
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

        public static void CreateDisbursementListDetails(long listId, DisbursementListDetails dDets)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"INSERT INTO DisbursementListDetails (listId,itemId,quantity)" +
                        "VALUES (@listId, @itemId, @quantity)";

                Console.WriteLine(q);
                SqlCommand cmd = new SqlCommand(q, conn);
                
                cmd.Parameters.AddWithValue("@listId", listId);
                cmd.Parameters.AddWithValue("@itemId", dDets.Item.ItemId);
                cmd.Parameters.AddWithValue("@quantity", dDets.Quantity);
                cmd.ExecuteNonQuery();
                
            }
        }
        public static void UpdateDetailById(long listId, DisbursementListDetails disbursementListDetails)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();


                string q = "UPDATE DisbursementListDetails SET quantity="+ disbursementListDetails.Quantity + " WHERE listId=" + listId + "AND itemId=" + disbursementListDetails.Item.ItemId;

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();


            }


        }
    }
}