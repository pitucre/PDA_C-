using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


namespace Mobile.PrinxChengShan.Util
{
    public class MysqlHelper
    {


        //private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MysqlHelper));

        //server=localhost;user id=root;password=root;persist security info=True;database=test
        //Data Source=127.0.0.1;port=3306;Initial Catalog=tsyw;user id=root;password=q2ii3sfc;Charset=gbk
        private static string conn_string = "server=zljy.work;user id=root;password=Wgh171319123;persist security info=True;database=prinx_problem";


        //server  数据库地址
        //user id  用户名
        //password  密码
        //persist security info  是否启用线程池
        //database  数据库名


        //空构造
        public MysqlHelper()
        {

        }
        /// <summary>
        /// 构造函数 初始化连接字符串
        /// </summary>
        /// <param name="conn_str">连接字符串/param>
        public MysqlHelper(string conn_str)
        {
            conn_string = conn_str;
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="cnn">数据库连接对象</param>
        private static void Open(MySqlConnection cnn)
        {
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="cnn">数据库连接对象</param>
        private static void Close(MySqlConnection cnn)
        {
            if (cnn != null)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                cnn.Dispose();
            }
        }
        /// <summary>
        /// 查询一个结果
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="pms">参数</param>
        /// <returns>返回结果</returns>
        public static object ExecuteScalar(string strSql, params MySqlParameter[] pms)
        {
            MySqlConnection cnn = new MySqlConnection(conn_string);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                Open(cnn);
                cmd = new MySqlCommand(strSql, cnn);
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                return cmd.ExecuteScalar();
            }
            catch (Exception e)
            {

                //System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally
            {
                cmd.Dispose();
                Close(cnn);
            }
            return null;
        }

        /// <summary>
        /// 执行查询语句(返回MySqlDataAdapter)
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="pms">参数</param>
        /// <returns>MySqlDataAdapter</returns>
        public static MySqlDataReader ExecuteReader(string strSql, params MySqlParameter[] pms)
        {
            MySqlConnection cnn = new MySqlConnection(conn_string);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                Open(cnn);
                cmd = new MySqlCommand(strSql, cnn);
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 查询（DataSet）
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>查询结果:DataSet</returns>
        /// foreach (DataRow item in dt.Rows){ }
        public DataTable GetDataTable(string strSql, params MySqlParameter[] pms)
        {
            MySqlConnection cnn = new MySqlConnection(conn_string);
            MySqlDataAdapter sda = new MySqlDataAdapter();
            try
            {
                Open(cnn);
                sda = new MySqlDataAdapter(strSql, cnn);
                if (pms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(pms);
                }
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                sda.Dispose();
                Close(cnn);
            }
        }

        /// <summary>
        /// 增加,删除,修改
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="pms">参数</param>
        /// <returns>返回受影响的行数。</returns>
        public static int ExecuteNonQuery(string strSql, params MySqlParameter[] pms)
        {
            MySqlConnection cnn = new MySqlConnection(conn_string);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                Open(cnn);
                cmd = new MySqlCommand(strSql, cnn);
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.Message);
                return 0;
            }
            finally
            {
                cmd.Dispose();
                Close(cnn);
            }

        }
        /// <summary>
        /// 执行返回多个查询时使用,返回List<T>类型
        /// </summary>
        /// <typeparam name="T">对应数据库的模型类</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="pms">参数</param>
        /// <returns>返回List<T>类型</returns>
        public List<T> QueryList<T>(string sql, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            List<T> list = new List<T>();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn_string))
            {
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }

                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    T _t = Activator.CreateInstance<T>();

                    PropertyInfo[] propertyInfo = _t.GetType().GetProperties();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        foreach (PropertyInfo info in propertyInfo)
                        {
                            if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                            {
                                if (dt.Rows[i][j] != DBNull.Value)
                                {
                                    info.SetValue(_t, dt.Rows[i][j], null);
                                }
                                else
                                {
                                    info.SetValue(_t, null, null);
                                }

                                break;
                            }
                        }
                    }
                    list.Add(_t);
                }
            }
            return list;

        }
    }
}



