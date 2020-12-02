using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
  public  class DBManager
    {
        public static string CS = string.Empty;
        public static IDbConnection DbConnect()
        {
            IDbConnection connection = new SqlConnection(CS);
            return connection;
        }

        public static SqlConnection DbConnect1()
        {
            SqlConnection connection = new SqlConnection(CS);
            return connection;
        }
    }
}
