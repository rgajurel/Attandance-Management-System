using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using System.Xml;

using System.Data.SqlClient;

namespace Infrastructure
{
    public class FeeDailyCollectionRepository : IFeeDailyCollectionRepository
    {
        public DataSet getAllData(FeeDailyCollection fee)
        {
            using (SqlConnection connection = DBManager.DbConnect1())
            {
                DataSet ds = new DataSet();
                try
                {
                    using (SqlCommand command = new SqlCommand("getDailyCollectionReport", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@dateFrom", SqlDbType.VarChar);
                        command.Parameters.Add("@dateTo", SqlDbType.VarChar);
                        command.Parameters.Add("@overAllStatus", SqlDbType.VarChar);
                        command.Parameters.Add("@session", SqlDbType.VarChar);
                        command.Parameters.Add("@faculty", SqlDbType.VarChar);
                        command.Parameters.Add("@class", SqlDbType.VarChar);
                        command.Parameters.Add("@section", SqlDbType.VarChar);
                        command.Parameters["@dateFrom"].Value = fee.DateFrom;
                        command.Parameters["@dateTo"].Value = fee.DateTo;
                        command.Parameters["@session"].Value = fee.Session;
                        if (fee.Overall == true)
                        {
                            command.Parameters["@overAllStatus"].Value = "true";
                            command.Parameters["@faculty"].Value = "";
                            command.Parameters["@class"].Value = "";
                            command.Parameters["@section"].Value = "";
                        }
                        else
                        {
                            command.Parameters["@overAllStatus"].Value = "false";
                            command.Parameters["@faculty"].Value = fee.Faculty;
                            command.Parameters["@class"].Value = fee.Class;
                            command.Parameters["@section"].Value = fee.Section;
                        }
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }   
                        command.Connection = connection;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds, "Table0");
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
                catch(Exception ex)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    ds = null;
                }
                return ds;
            }
        }
    }
}
