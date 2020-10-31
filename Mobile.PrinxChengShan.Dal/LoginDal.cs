using DataOperate.Net;
using Mobile.PrinxChengShan.Model;
using System.Data.SqlClient;

namespace Mobile.PrinxChengShan.Dal
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class LoginDal
    {
        private MsSqlHelper db = null;
        public LoginDal()
        {
            db = new MsSqlHelper();
        }
        /// <summary>
        /// 验证用户登录信息获取信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LSFW_EMPLOYEE GetUserLogin(string loginName, string password)
        {
            try
            {
                string strSql = string.Format(@" SELECT PASSWORD,ID,FAC,DEPNAM,NAME,LOGINNAME,[PASSWORD],POSNAM,PHONE,MAIL,LEAYN,ISLOGIN,HPIC,LCCID,ENAM,ETIM FROM LSFW_EMPLOYEE (NOLOCK) WHERE LOGINNAME = '{0}' AND PASSWORD = '{1}' AND LEAYN = 'N'", loginName, password);
                LSFW_EMPLOYEE model = new LSFW_EMPLOYEE();
                using (SqlDataReader read = db.ExecuteReader(strSql))
                {
                    if (read.Read())
                    {
                        model.ID = read["ID"].ToString();
                        model.FAC = read["FAC"].ToString();                      
                        model.ENAM = read["ENAM"].ToString();
                        model.DEPNAM = read["DEPNAM"].ToString();
                        model.NAME = read["NAME"].ToString();
                        model.LOGINNAME = read["LOGINNAME"].ToString();
                        model.POSNAM = read["POSNAM"].ToString();
                        model.PHONE = read["PHONE"].ToString();
                        model.MAIL = read["MAIL"].ToString();
                        model.LEAYN = read["LEAYN"].ToString();
                        model.ISLOGIN = read["ISLOGIN"].ToString();
                        model.PASSWORD = read["PASSWORD"].ToString();
                    }
                    read.Dispose();
                }
                return model;
            }
            catch { throw; }
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool UpdatePassword(string Id, string loginName, string password)
        {
            try
            {
                string strSql = string.Format(@" UPDATE LSFW_EMPLOYEE SET 
[PASSWORD] = '{1}'
WHERE LOGINNAME = '{0}' AND ID = '{2}'", loginName, password, Id);
                return db.ExecuteNonQuery(strSql) > 0;
            }
            catch { throw; }
        }
    }
}

