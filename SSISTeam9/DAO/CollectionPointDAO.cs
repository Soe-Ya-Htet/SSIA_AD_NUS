using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using System.Data.SqlClient;

namespace SSISTeam9.DAO
{
    public class CollectionPointDAO
    {
        internal static List<CollectionPoint> GetAllCollectionPoints()
        {
            List<CollectionPoint> collectionPoints = new List<CollectionPoint>();
            CollectionPoint collectionPoint = null;
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from CollectionPoint";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    collectionPoint = new CollectionPoint()
                    {
                        PlacedId = (long)reader["placeId"],
                        Name = (string)reader["name"],
                        Time = (TimeSpan)reader["time"]
                    };
                    collectionPoints.Add(collectionPoint);
                }
            }
            return collectionPoints;
        }
    }
}