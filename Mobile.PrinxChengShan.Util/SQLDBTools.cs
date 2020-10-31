using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mobile.PrinxChengShan.Util
{
    public class SQLDBTools
    {

        public static string CONN_STRING_NON_DTC
        {
            get
            {
                return "Data Source=10.151.65.11;Initial Catalog=FixedAssetBarcode;User ID=RCFixsa;Password=asxiFCR";
                //Persist Security Info=True;
                //return "Data Source=10.151.65.14;Initial Catalog=FixedAssetBarcode;Persist Security Info=True;User ID=sa;Password=5ql123";
            }
        }

        public static string CONN_STRING_NON_Ret
        {
            get
            {
                //return "Data Source=" + cRWIni.ServerIP() + ";Initial Catalog=master;Persist Security Info=True;User ID=" + cRWIni.DBUserName() + ";Password=" + cRWIni.DBUserPassWord() + "";
                //return "Server=" + cRWIni.ServerIP() + ";Database=master;user id=" + cRWIni.DBUserName() + ";Password=" + cRWIni.DBUserPassWord() + ";Trusted_Connection=False";
                return "";
            }
        }

        //public static readonly string CONN_STRING_NON_DTC = cDataBase.sqlcn;
        public SQLDBTools()
        {
            //sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
        }

        public static bool TestSQL()
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
                sqlConn.Open();
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }
        #region 检索单值
        //*****************************************************************************************//
        /// <summary>
        /// 从数据库中检索单个非数值型数据
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static string GetStr(string strSQL)
        {
            string strTmp;
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlCommand sqlCmd = sqlConn.CreateCommand();

            sqlCmd.CommandText = strSQL;
            sqlCmd.CommandType = CommandType.Text;

            try
            {
                sqlConn.Open();
                strTmp = Convert.ToString(sqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }


            return strTmp;
        }

        //*****************************************************************************************//
        /// <summary>
        ///从数据库中检索单个数值型数据 
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static int GetInt(string strSQL)
        {
            int intTmp;
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlCommand sqlCmd = sqlConn.CreateCommand();
            sqlCmd.CommandText = strSQL;
            sqlCmd.CommandType = CommandType.Text;

            sqlConn.Open();
            intTmp = Convert.ToInt32(sqlCmd.ExecuteScalar());
            sqlConn.Close();
            return intTmp;
        }
        /// <summary>
        ///从数据库中检索单个数值型数据 
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static long GetLong(string strSQL)
        {
            long nRet;
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlCommand sqlCmd = sqlConn.CreateCommand();
            sqlCmd.CommandText = strSQL;
            sqlCmd.CommandType = CommandType.Text;

            sqlConn.Open();
            nRet = Convert.ToInt64(sqlCmd.ExecuteScalar());
            sqlConn.Close();
            return nRet;
        }

        //*****************************************************************************************//

        /// <summary>
        /// 检索数据库中符合某个条件的行数
        /// </summary>
        /// <param name="strVal">要检索的值</param>
        /// <param name="strFld">对应的表字段</param>
        /// <param name="strTblName">对应的表</param>
        /// <returns>返回整型值</returns>
        public static int GetCount(string strVal, string strFld, string strTblName)
        {

            string strSQL = "";
            int intTmp = 0;
            strSQL = "select Count(*) as cnt from " + strTblName + " where " + strFld + "='" + strVal + "'";

            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlCommand sqlCmd = sqlConn.CreateCommand();
            sqlCmd.CommandText = strSQL;
            sqlCmd.CommandType = CommandType.Text;

            sqlConn.Open();
            intTmp = Convert.ToInt32(sqlCmd.ExecuteScalar());
            sqlConn.Close();
            return intTmp;

        }
        //*****************************************************************************************//
        /// <summary>
        /// 检索数据库中某个整型值的数量
        /// </summary>
        /// <param name="intValue">整型数值对象</param>
        /// <param name="strFld">对应的表字段</param>
        /// <param name="strTblName">对应的表</param>
        /// <returns>返回整形值</returns>
        public static int GetCount(int intValue, string strFld, string strTblName)
        {
            int intTmp;
            string strSQL;

            strSQL = "select count(*) as cnt from " + strTblName + " where " + strFld + "=" + intValue + "";

            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlCommand sqlCmd = sqlConn.CreateCommand();
            sqlCmd.CommandText = strSQL;
            sqlCmd.CommandType = CommandType.Text;

            sqlConn.Open();
            intTmp = Convert.ToInt32(sqlCmd.ExecuteScalar());
            sqlConn.Close();

            return intTmp;

        }
        //*****************************************************************************************//

        /// <summary>
        /// 检索数据库中数据的数量
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns></returns>
        public static int GetCount(string strSQL)
        {
            int intTmp;

            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlCommand sqlCmd = sqlConn.CreateCommand();
            sqlCmd.CommandText = strSQL;
            sqlCmd.CommandType = CommandType.Text;
            try
            {
                sqlConn.Open();
                intTmp = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
            return intTmp;
        }

        public static int GetCount(string strSQL, int Flag)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            try
            {
                SqlDataAdapter objAdapter = new SqlDataAdapter(strSQL, sqlConn);
                sqlConn.Open();
                objAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
        }
        #endregion

        #region 检索记录集
        //*****************************************************************************************//
        /// <summary>
        /// DataSet获取数据
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="strTblName">内存表名</param>
        /// <returns></returns>
        public static DataSet GetDS(string strSQL, string strTblName)
        {
            DataSet myds = new DataSet();
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
            sqlConn.Open();
            sqlAdapter.Fill(myds, strTblName);
            sqlConn.Close();
            return myds;

        }

        /// <summary>
        /// DataSet获取数据
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="StartIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="strTblName"></param>
        /// <returns></returns>
        public static DataSet GetDS(string strSQL, int StartIndex, int PageSize, string strTblName)
        {
            DataSet myds = new DataSet();
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
            sqlConn.Open();
            sqlAdapter.Fill(myds, StartIndex, PageSize, strTblName);
            sqlConn.Close();
            return myds;
        }
        #endregion

        #region 检索单条数据
        //*****************************************************************************************//
        /// <summary>
        /// DataRow获取单条数据
        /// </summary>
        /// <param name="strKey">检索条件</param>
        /// <param name="strFld">数据库中的字段</param>
        /// <param name="strTblName">表名</param>
        public static DataRow GetRow(string strKey, string strFld, string strTblName)
        {
            DataRow objRow;
            DataTable dt = new DataTable();
            string strSQL;

            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            strSQL = "select * from " + strTblName + " where " + strFld + "='" + strKey + "'";
            try
            {
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
                sqlConn.Open();
                sqlAdapter.Fill(dt);
                objRow = dt.NewRow();
                if (dt.Rows.Count == 1)
                {
                    objRow = dt.Rows[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }

            return objRow;
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="szQuerySQL"></param>
        /// <param name="ParamNames"></param>
        /// <param name="ParamValues"></param>
        /// <returns></returns>
        public static DataRow GetRow(string strQuerySQL, string[] ParamNames, object[] ParamValues)
        {
            DataRow objRow = null;
            DataTable dt = new DataTable();

            if (ParamNames.Length != ParamValues.Length)
                return null;

            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = new SqlCommand();
            sqlAdapter.SelectCommand.Connection = sqlConn;
            sqlAdapter.SelectCommand.CommandText = strQuerySQL;

            for (int i = 0; i < ParamValues.Length; i++)
                sqlAdapter.SelectCommand.Parameters.AddWithValue(ParamNames[i], ParamValues[i]);
            try
            {
                sqlAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            objRow = dt.NewRow();

            if (dt.Rows.Count == 1)
                objRow = dt.Rows[0];
            else
                objRow = null;
            return objRow;
        }

        /// <summary>
        /// DataRow获取单条数据
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        public static DataRow GetRow(string strSQL)
        {
            DataRow objRow;
            DataTable dt = new DataTable();

            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
            try
            {
                sqlConn.Open();
                sqlAdapter.Fill(dt);
                objRow = dt.NewRow();
                if (dt.Rows.Count == 1)
                {
                    objRow = dt.Rows[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }

            return objRow;
        }
        //*****************************************************************************************//
        /// <summary>
        /// 数组获取单条数据
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="aryFld"></param>
        /// <param name="aryVal"></param>
        public static void GetSingleRec(string strSQL, string[] aryFld, ref string[] aryVal)
        {
            string strTmp = "";
            string strTmp2;
            int CNT, i;
            CNT = aryFld.Length;

            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn);
            sqlConn.Open();
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            sqlReader.Read();
            for (i = 0; i < CNT; i++)
            {
                strTmp2 = sqlReader[aryFld[i]].ToString();
                strTmp2 = strTmp2.Replace("|", "");
                if (i == 0)
                {
                    strTmp = strTmp2;
                }
                else
                {
                    strTmp = strTmp + "|" + strTmp2;
                }
            }
            aryVal = strTmp.Split(Convert.ToChar("|"));
            sqlConn.Close();
        }
        #endregion

        #region 检索数据表
        //*****************************************************************************************//
        /// <summary>
        /// DataSet获取数据
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetDS(string strSQL)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            //sqlConn.ConnectionString = CONN_STRING_NON_DTC;
            try
            {
                SqlDataAdapter objAdapter = new SqlDataAdapter(strSQL, sqlConn);
                sqlConn.Open();
                objAdapter.Fill(ds);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
            return ds;
        }

        public static DataTable GetTable(string strSQL)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            try
            {
                SqlDataAdapter objAdapter = new SqlDataAdapter(strSQL, sqlConn);
                sqlConn.Open();
                objAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
            return dt;
        }

        #endregion

        #region 检索指定集合一列值
        /// <summary>
        /// 获取一列值
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="strFld"></param>
        /// <param name="aryVal"></param>
        public static string GetFldVal(string strSQL, string strFld, ref string[] aryVal)
        {
            string strTmp = "";
            string strTmp2 = "";
            int i = 0;

            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn);
            sqlConn.Open();
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                strTmp2 = sqlReader[strFld].ToString();
                strTmp2 = strTmp2.Replace("|", "");
                if (i == 0)
                {
                    strTmp = strTmp2;
                }
                else
                {
                    strTmp = strTmp + "|" + strTmp2;
                }
                i++;
            }
            aryVal = strTmp.Split(Convert.ToChar("|"));

            sqlConn.Close();
            return strTmp;
        }
        #endregion

        /// <summary>
        /// 数据增删改
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        public static bool ExecSQL(string strSQL)
        {
            bool ret = false;
            SqlCommand sqlCmd = null;
            SqlConnection sqlConn = new SqlConnection(CONN_STRING_NON_DTC);
            try
            {
                sqlCmd = new SqlCommand(strSQL, sqlConn);
                sqlConn.Open();
                if (sqlCmd.ExecuteNonQuery() > 0)
                    ret = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                sqlCmd.Dispose();
                sqlConn.Close();
            }
            return ret;
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static string ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(CONN_STRING_NON_DTC))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return "OK";
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        //throw ex;
                        return ex.Message;
                    }
                }
            }
        }
    }
}
