using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace Test
{

    class DbUtil
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Test2ConnectionString"].ConnectionString;
        }

        public SqlConnection GetSqlConnection(string connectionString)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                if (connection != null)
                    connection.Dispose();
            }
            return connection;
        }
    }
}