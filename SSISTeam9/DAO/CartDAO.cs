using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class CartDAO
    {
        public static void SaveCart(Cart cart)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO Cart (empId,itemId,quantity)" +
                            "VALUES (" + cart.Employee.EmpId +
                            "," + cart.Item.ItemId +
                            "," + cart.Quantity + ")";
                Console.WriteLine(q);
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateCart(Cart cart)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"Update Cart set quantity=@quantity Where empId=@empId And itemId=@itemId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@quantity", cart.Quantity);
                cmd.Parameters.AddWithValue("@empId", cart.Quantity);
                cmd.Parameters.AddWithValue("@itemId", cart.Quantity);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Cart> GetAllCart()
        {
            List<Cart> carts = new List<Cart>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Cart";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employee e = new Employee()
                    {
                        EmpId = (long)reader["empId"]
                    };

                    Inventory i = new Inventory()
                    {
                        ItemId = (long)reader["itemId"]
                    };

                    Cart cart = new Cart()
                    {
                        Quantity = (int)reader["quantity"],
                        Employee = e,
                        Item = i
                    };
                    carts.Add(cart);
                }
            }
            return carts;
        }
    }
}