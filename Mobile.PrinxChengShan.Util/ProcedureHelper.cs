using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Mobile.PrinxChengShan.Util
{
    public class ProcedureHelper
    {
        public static string Connection = ConfigurationManager.ConnectionStrings["DatabaseConnectivity"].ConnectionString;
        public static string ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] pars)
        {
            SqlConnection connection = new SqlConnection(Connection);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = storedProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            if (pars != null)
            {
                cmd.Parameters.AddRange(pars);
            }
            try
            {
                connection.Open();
                return (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
