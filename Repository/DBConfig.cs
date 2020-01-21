using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Repository
{
    public class DBConfig
    {
        public readonly static string DefaultSqlConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";

        public static IDbConnection GetSqlConnection(string sqlConnectionString = null) {
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnectionString;
            }
            IDbConnection conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }

    }
}
