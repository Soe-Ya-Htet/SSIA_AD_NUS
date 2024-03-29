﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class RequisitionDetailsDAO
    {
        public static void SaveRequisitionDetails(List<RequisitionDetails> reqDetailsList)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO RequisitionDetails (reqId,itemId,quantity,balance)" +
                            "VALUES (@reqId, @itemId, @quantity, @balance)";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@reqId", DbType.Int64);
                cmd.Parameters.AddWithValue("@itemId", DbType.Int64);
                cmd.Parameters.AddWithValue("@quantity", DbType.Int32);
                cmd.Parameters.AddWithValue("@balance", DbType.Int32);
                foreach (var detail in reqDetailsList)
                {
                    cmd.Parameters[0].Value = detail.Requisition.ReqId;
                    cmd.Parameters[1].Value = detail.Item.ItemId;
                    cmd.Parameters[2].Value = detail.Quantity;
                    cmd.Parameters[3].Value = detail.Balance;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<RequisitionDetails> GetRequisitionDetailsByReqId(long reqId)
        {
            List<RequisitionDetails> reqDetails = new List<RequisitionDetails>();
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT r.*,i.flag from RequisitionDetails r, Inventory i where r.itemId=i.itemId and reqId=" + reqId;
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory i = new Inventory()
                    {
                        ItemId = (long)reader["itemId"],
                        Flag = (int)reader["flag"]
                    };
                    RequisitionDetails requisitionDetail = new RequisitionDetails()
                    {
                        Quantity = (int)reader["quantity"],
                        Balance = (int)reader["balance"],
                        Item = i
                    };

                    reqDetails.Add(requisitionDetail);
                }
            }
            return reqDetails;

        }

        public static List<RequisitionDetails> GetRemainingRequisitionDetailsByReqId(long reqId)
        {
            List<RequisitionDetails> reqDetails = new List<RequisitionDetails>();
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from RequisitionDetails WHERE reqId=@reqId AND balance!=0";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@reqId", reqId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory i = new Inventory()
                    {
                        ItemId = (long)reader["itemId"],
                    };
                    RequisitionDetails requisitionDetail = new RequisitionDetails()
                    {
                        Quantity = (int)reader["quantity"],
                        Balance = (int)reader["balance"],
                        Item = i
                    };

                    reqDetails.Add(requisitionDetail);
                }
            }
            return reqDetails;

        }

        internal static void UpdateBalanceAmount(long reqId, object itemId, int qty)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE RequisitionDetails SET balance=@qty where itemId=@itemId AND reqId=@reqId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@itemId", itemId);
                cmd.Parameters.AddWithValue("@reqId", reqId);

                cmd.ExecuteNonQuery();

            }
        }
    }
}